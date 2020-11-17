using System;
using System.Reflection;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using DataAccess.Attributes;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using Exceptions.DataAccess;


namespace DataAccess.Repos
{
    internal abstract class RepoBase<TEntity> : IRepo<TEntity> where TEntity : EntityBase, new()
    {
        #region Fields

        private static Regex UpperLetterPattern = new Regex("[A-Z]+");

        #endregion

        #region Properties

        public bool Lazy { get; set; } 

        protected abstract string ConnectionString { get; }

        protected static string TableName { get; set; }

        protected static string KeyColumnName { get; set; }
        protected static PropertyInfo KeyProperty { get; set; }

        protected static IEnumerable<string> Columns { get; set; }
        protected static IEnumerable<PropertyInfo> Properties { get; set; }

        protected static IEnumerable<string> ColumnsExceptKey { get; set; }
        protected static IEnumerable<PropertyInfo> PropertiesExceptKey { get; set; }

        #endregion

        #region Constructors

        public RepoBase()
        {
            InitMetaData();
        }

        #endregion

        #region Methods
        #region Public

        public async Task<IEnumerable<TEntity>> Get(int limit)
        {
            return await GetLimited(limit);
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await GetLimited(-1);
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, object>> expression, object value, int limit)
        {
            return await GetLimited(expression, value, limit);
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, object>> expression, object value)
        {
            return await GetLimited(expression, value, -1);
        }

        public async Task<TEntity> GetById(int id)
        {
            return await FirstOrDefault(entity => entity.Id, id);
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, object>> expression, object value)
        {
            IEnumerable<TEntity> list = await GetLimited(expression, value, 1);
            return list.FirstOrDefault();
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            int id = await InsertEntity(entity);
            return await FirstOrDefault(e => e.Id, id);
        }

        /// <summary>
        /// override it to update dependent entities
        /// </summary>
        public virtual async Task<TEntity> Update(TEntity entity)
        {
            int id = (int)KeyProperty.GetValue(entity);
            TEntity current = await FirstOrDefault(stored => stored.Id, id);

            if (current == null)
                throw new NotFoundException("entity with such id");

            IDictionary<string, object> changedColumns = new Dictionary<string, object>();
            foreach (PropertyInfo property in PropertiesExceptKey)
            {
                object storedValue = property.GetValue(current);
                object updatedValue = property.GetValue(entity);

                if (storedValue.Equals(updatedValue))
                    continue;

                changedColumns.Add(GetColumnByProperty(property), updatedValue);
            }

            if (changedColumns.Count == 0)
                return current;

            IEnumerable<string> changeQueryItems = changedColumns.Select(change => $"{change.Key} = @{change.Key}");
            string setString = string.Join(", ", changeQueryItems);

            string updateText = $"UPDATE {TableName}\n" +
                                $"SET {setString}\n" +
                                $"WHERE {KeyColumnName} = {id};";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand updater = new SqlCommand(updateText, connection))
            {
                foreach (KeyValuePair<string, object> changedColumn in changedColumns)
                    updater.Parameters.AddWithValue($"@{changedColumn.Key}", changedColumn.Value);

                connection.Open();
                await updater.ExecuteNonQueryAsync();
            }

            return await FirstOrDefault(updated => updated.Id, id);
        }

        /// <summary>
        /// override it to delete dependent entities
        /// </summary>
        public virtual async Task Delete(int id)
        {
            string deleteText = $"DELETE FROM {TableName}\n" +
                                $"WHERE {KeyColumnName} = @id;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand deleter = new SqlCommand(deleteText, connection))
            {
                deleter.Parameters.AddWithValue("@id", id);
                connection.Open();
                await deleter.ExecuteNonQueryAsync();
            }
        }

        #endregion

        #region Protected

        /*protected abstract Task<TEntity> LoadDependencies(TEntity entity);
        protected async Task<IEnumerable<TEntity>> LoadDependencies(IEnumerable<TEntity> entities)
        {
            //millions of queries
            List<TEntity> entitiesList = new List<TEntity>();
            foreach (TEntity entity in entities)
                entitiesList.Add(await LoadDependencies(entity));
            return entitiesList;
        }*/

        /// <summary>
        /// override it to insert dependent entities
        /// </summary>
        protected virtual async Task<int> InsertEntity(TEntity entity)
        {
            string columns = string.Join(",", ColumnsExceptKey.Select(column => $"\"{column}\""));

            IDictionary<string, object> parametersValues = GetParametersValues(ColumnsExceptKey, entity);
            string parametersString = string.Join(",", parametersValues.Keys);

            string insertText = $"INSERT INTO {TableName}({columns})\n" +
                                $"OUTPUT INSERTED.ID\n" +
                                $"VALUES({parametersString});";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand inserter = new SqlCommand(insertText, connection))
            {
                foreach (KeyValuePair<string, object> parameterValue in parametersValues)
                    inserter.Parameters.AddWithValue(parameterValue.Key, parameterValue.Value);

                connection.Open();
                return (int)await inserter.ExecuteScalarAsync();
            }
        }

        #endregion

        #region Private

        #region Static
        private static void InitMetaData()
        {
            Properties = typeof(TEntity).GetProperties().Where(property => property.GetCustomAttribute<IgnoreAttribute>() == null);
            Columns = Properties.Select(property => GetColumnByProperty(property)).ToList();

            KeyProperty = GetKeyProperty();
            KeyColumnName = GetColumnByProperty(KeyProperty);

            PropertiesExceptKey = Properties.Where(property => property.Name != KeyProperty.Name).ToList();
            ColumnsExceptKey = Columns.Where(column => column != KeyColumnName).ToList();

            TableName = typeof(TEntity).GetCustomAttribute<TableAttribute>()?.Name ?? ParseDefaultTableName();
        }

        private static string ParseDefaultName(string name)
        {
            return UpperLetterPattern.Replace(name, "_$0").ToLower().Trim('_');
        }

        private static string ParseDefaultTableName()
        {
            return ParseDefaultName(
                typeof(TEntity).Name.Replace("Entity", "")
            );
        }

        private static PropertyInfo GetKeyProperty()
        {
            return Properties.FirstOrDefault(property => property.GetCustomAttribute<KeyAttribute>() != null) ??
                   Properties.FirstOrDefault(property => property.Name.ToLower() == "id") ?? 
                   throw new InvalidOperationException("No key property found");
        }

        private static IDictionary<string, object> GetParametersValues(IEnumerable<string> columns, TEntity entity)
        {
            return columns.ToDictionary(
                column => $"@{column}",
                column => GetPropertyByColumn(column).GetValue(entity)
            );
        }

        private static string ParseDefaultPropertyNameToColumnName(PropertyInfo property)
        {
            return ParseDefaultName(property.Name);
        }

        private static string GetColumnByProperty(PropertyInfo property)
        {
            if (property == null)
                return null;

            return property.GetCustomAttribute<ColumnAttribute>()?.Name ?? 
                   ParseDefaultPropertyNameToColumnName(property);
        }

        private static PropertyInfo GetPropertyByColumn(string column)
        {
            return Properties.FirstOrDefault(property => property.GetCustomAttribute<ColumnAttribute>()?.Name == column) ??
                   Properties.FirstOrDefault(property => ParseDefaultPropertyNameToColumnName(property) == column) ??
                   throw new InvalidOperationException("No property for column found");
        }

        private static string GetLimitString(int limit)
        {
            return limit > 0 ? $" TOP {limit}" : "";
        }

        private static IEnumerable<string> GetSelectParameters()
        {
            Properties typeof(TEntity).GetProperties()
        }
        #endregion

        private TEntity Fetch(SqlDataReader reader)
        {
            TEntity entity = new TEntity();

            for (int i = 0; i < reader.FieldCount; ++i)
            {
                string name = reader.GetName(i);
                PropertyInfo property = GetPropertyByColumn(name);

                if (property != null)
                {
                    object converted = Convert.ChangeType(reader[name], property.PropertyType);
                    property.SetValue(entity, converted);
                }
            }

            return entity;
        }

        private async Task<IEnumerable<TEntity>> FetchList(SqlDataReader reader)
        {
            List<TEntity> entities = new List<TEntity>();

            while (await reader.ReadAsync())
            {
                TEntity entity = Fetch(reader);
                entities.Add(entity);
            }

            return entities;
        }

        private string GetColumnByExpression(Expression<Func<TEntity, object>> expression)
        {
            MemberExpression memberExpression = null;

            if (expression.Body.NodeType == ExpressionType.MemberAccess)
                memberExpression = expression.Body as MemberExpression;
            else if (expression.Body.NodeType == ExpressionType.Convert)
                memberExpression = (expression.Body as UnaryExpression)?.Operand as MemberExpression;

            if (memberExpression == null)
                return null;

            return GetColumnByProperty(memberExpression.Member as PropertyInfo);
        }

        private async Task<IEnumerable<TEntity>> GetLimited(Expression<Func<TEntity, object>> expression, object value, int limit)
        {
            string column = GetColumnByExpression(expression);
            string selectText = $"SELECT {GetLimitString(limit)} * FROM {TableName} WHERE {column} = @value;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand selector = new SqlCommand(selectText, connection))
            {
                selector.Parameters.AddWithValue("@value", value);
                connection.Open();
                return await GetQueryResult(selector);
            }
        }

        private async Task<IEnumerable<TEntity>> GetLimited(int limit)
        {
            string selectText = $"SELECT {GetLimitString(limit)} * FROM {TableName};";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand selector = new SqlCommand(selectText, connection))
            {
                connection.Open();
                return await GetQueryResult(selector);
            }
        }

        private async Task<IEnumerable<TEntity>> GetQueryResult(SqlCommand command)
        {
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                IEnumerable<TEntity> entities = await FetchList(reader);
                if (Lazy)
                    return entities;
                return await LoadDependencies(entities);
            }
        }

        #endregion
        #endregion
    }
}

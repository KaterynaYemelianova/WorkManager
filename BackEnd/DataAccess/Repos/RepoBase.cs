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
using Autofac;

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

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await GetQuery(-1);
        }

        public async Task<IEnumerable<TEntity>> Get(int limit)
        {
            return await GetQuery(limit);
        }

        public async Task<IDictionary<T, List<TEntity>>> Get<T>(Expression<Func<TEntity, T>> expression, params T[] values)
        {
            return await Get(expression, -1, values);
        }

        public async Task<IDictionary<T, List<TEntity>>> Get<T>(Expression<Func<TEntity, T>> expression, int limit, params T[] values)
        {
            IEnumerable<TEntity> entities = await GetQuery(expression, limit, values);
            return entities.GroupBy(expression.Compile()).ToDictionary(
                group => group.Key,
                group => group.ToList()
            );
        }

        public async Task<IEnumerable<TEntity>> Get<T>(IDictionary<Expression<Func<TEntity, T>>, T> where)
        {
            return await GetQuery(where);
        }

        public async Task<TEntity> GetById(int id)
        {
            return await FirstOrDefault(entity => entity.Id, id);
        }

        public async Task<IEnumerable<TEntity>> GetByIds(params int[] ids)
        {
            return await GetQuery(entity => entity.Id, -1, ids);
        }

        public async Task<IDictionary<int, TEntity>> GetDictionaryByIds(params int[] ids)
        {
            IEnumerable<TEntity> entities = await GetByIds(ids);
            return entities.ToDictionary(
                entity => entity.Id,
                entity => entity
            );
        }

        public async Task<TEntity> FirstOrDefault<T>(Expression<Func<TEntity, T>> expression, T value)
        {
            IEnumerable<TEntity> list = await GetQuery(expression, 1, value);
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
        public virtual async Task Delete(params int[] ids)
        {
            await Delete(entity => entity.Id, ids);
        }

        public async Task Delete<T>(Expression<Func<TEntity, T>> expression, params T[] values)
        {
            await DeleteQuery(expression, values);
        }

        public async Task Delete<T>(IDictionary<Expression<Func<TEntity, T>>, T> wheres)
        {
            await DeleteQuery(wheres);   
        }

        #endregion

        #region Protected

        protected abstract Task<IEnumerable<TEntity>> LoadDependencies(IEnumerable<TEntity> entities);

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

        protected async Task<IEnumerable<T>> UpdateCollection<T>(IEnumerable<T> oldCollection, IEnumerable<T> newCollection) where T : EntityBase, new()
        {
            IRepo<T> collectionRepo = DataAccessDependencyHolder.Dependencies.Resolve<IRepo<T>>();

            if (newCollection == null)
                newCollection = new List<T>();

            IEnumerable<T> toUpdate = oldCollection.Intersect(newCollection);
            IEnumerable<T> toDelete = oldCollection.Except(newCollection);
            IEnumerable<T> toInsert = newCollection.Except(oldCollection);

            List<T> result = new List<T>();

            foreach (T updating in toUpdate)
            {
                T updated = await collectionRepo.Update(updating);
                result.Add(updated);
            }

            await collectionRepo.Delete(
                toDelete.Select(deleting => deleting.Id).ToArray()
            );

            foreach(T inserting in toInsert)
            {
                T inserted = await collectionRepo.Insert(inserting);
                result.Add(inserted);
            }

            return result;
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

        private string GetColumnByExpression<T>(Expression<Func<TEntity, T>> expression)
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

        private async Task<IEnumerable<TEntity>> GetQuery<T>(IDictionary<Expression<Func<TEntity, T>>, T> wheres)
        {
            if (wheres.Count == 0)
                return await Get();

            Dictionary<string, T> columnsWheres = wheres.ToDictionary(
                where => GetColumnByExpression(where.Key),
                where => where.Value
            );

            string whereLine = string.Join(
                " AND ", 
                columnsWheres.Select(
                    columnWhere => $"{columnWhere.Key} = @{columnWhere.Key}"
                )
            );

            string selectText = $"SELECT * FROM {TableName} WHERE {whereLine};";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand selector = new SqlCommand(selectText, connection))
            {
                foreach(KeyValuePair<string, T> columnWhere in columnsWheres)
                    selector.Parameters.AddWithValue($"@{columnWhere.Key}", columnWhere.Value);

                connection.Open();
                return await GetQueryResult(selector);
            }
        }

        private async Task<IEnumerable<TEntity>> GetQuery<T>(Expression<Func<TEntity, T>> expression, int limit, params T[] values)
        {
            if (values.Length == 0)
                return new List<TEntity>();

            string column = GetColumnByExpression(expression);
            string valueParameters = string.Join(",", Enumerable.Range(1, values.Length).Select(i => $"@value{i}"));
            string selectText = $"SELECT {GetLimitString(limit)} * FROM {TableName} WHERE {column} IN ({valueParameters});";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand selector = new SqlCommand(selectText, connection))
            {
                for(int i = 0; i < values.Length; i++)
                    selector.Parameters.AddWithValue($"@value{i + 1}", values[i]);

                connection.Open();
                return await GetQueryResult(selector);
            }
        }

        private async Task<IEnumerable<TEntity>> GetQuery(int limit)
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

        private async Task DeleteQuery<T>(Expression<Func<TEntity, T>> expression, params T[] values)
        {
            if (values.Length == 0)
                return;

            string column = GetColumnByExpression(expression);
            string parametersValues = string.Join(",", Enumerable.Range(1, values.Length).Select(i => $"@value{i}"));
            string deleteText = $"DELETE FROM {TableName}\n" +
                                $"WHERE {column} IN ({parametersValues});";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand deleter = new SqlCommand(deleteText, connection))
            {
                for (int i = 0; i < values.Length; ++i)
                    deleter.Parameters.AddWithValue($"@value{i + 1}", values[i]);

                connection.Open();
                await deleter.ExecuteNonQueryAsync();
            }
        }

        private async Task DeleteQuery<T>(IDictionary<Expression<Func<TEntity, T>>, T> wheres)
        {
            if (wheres.Count == 0)
                return;

            Dictionary<string, T> columnsWheres = wheres.ToDictionary(
                where => GetColumnByExpression(where.Key),
                where => where.Value
            );

            string whereLine = string.Join(
                " AND ",
                columnsWheres.Select(
                    columnWhere => $"{columnWhere.Key} = @{columnWhere.Key}"
                )
            );

            string deleteText = $"DELETE FROM {TableName} WHERE {whereLine};";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand deleter = new SqlCommand(deleteText, connection))
            {
                foreach (KeyValuePair<string, T> columnWhere in columnsWheres)
                    deleter.Parameters.AddWithValue($"@{columnWhere.Key}", columnWhere.Value);

                connection.Open();
                await deleter.ExecuteNonQueryAsync();
            }
        }

        #endregion
        #endregion
    }
}

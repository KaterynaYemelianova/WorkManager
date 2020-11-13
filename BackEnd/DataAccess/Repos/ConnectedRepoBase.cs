using DataAccess.Entities;

namespace DataAccess.Repos
{
    internal class ConnectedRepoBase<TEntity> : RepoBase<TEntity> where TEntity : EntityBase, new()
    {
        private const string CONNECTION_STRING = "workstation id=workmanagerdb.mssql.somee.com;packet size=4096;user id=k_emelianova_SQLLogin_1;pwd=zsjm2nuzc6;data source=workmanagerdb.mssql.somee.com;persist security info=False;initial catalog=workmanagerdb";
        public ConnectedRepoBase() : base(CONNECTION_STRING)
        {
        }
    }
}

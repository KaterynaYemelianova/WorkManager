using DataAccess.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    internal abstract class ConnectedRepoBase<TEntity> : RepoBase<TEntity> where TEntity : EntityBase, new()
    {
        protected override string ConnectionString => 
            "workstation id=workmanagerdb.mssql.somee.com;" +
            "packet size=4096;" +
            "user id=k_emelianova_SQLLogin_1;" +
            "pwd=zsjm2nuzc6;" +
            "data source=workmanagerdb.mssql.somee.com;" +
            "persist security info=False;" +
            "initial catalog=workmanagerdb";
    }
}

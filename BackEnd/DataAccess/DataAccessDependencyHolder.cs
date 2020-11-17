using Autofac;
using DataAccess.Entities;
using DataAccess.RepoContracts;
using DataAccess.Repos;

namespace DataAccess
{
    public static class DataAccessDependencyHolder
    {
        private static IContainer denendencies = null;

        public static IContainer Dependencies
        {
            get
            {
                if (denendencies == null)
                    denendencies = BuildDependencies();
                return denendencies;
            }
        }

        private static IContainer BuildDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<AccountRepo>().As<IAccountRepo>().SingleInstance();
            builder.RegisterType<CompanyRepo>().As<ICompanyRepo>().SingleInstance();
            builder.RegisterType<RoomRepo>().As<IRoomRepo>().SingleInstance();

            builder.RegisterType<NoDependenciesRepo<RoleEntity>>().AsSelf().SingleInstance();
            builder.RegisterType<NoDependenciesRepo<AccountCompanyRoleEntity>>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}

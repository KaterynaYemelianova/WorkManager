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

            builder.RegisterType<AccountRepo>().As<IAccountRepo>().As<IRepo<AccountEntity>>().SingleInstance();
            builder.RegisterType<CompanyRepo>().As<ICompanyRepo>().As<IRepo<CompanyEntity>>().SingleInstance();
            builder.RegisterType<RoomRepo>().As<IRoomRepo>().As<IRepo<RoomEntity>>().SingleInstance();

            builder.RegisterType<NoDependenciesRepo<RoleEntity>>().AsSelf().As<IRepo<RoleEntity>>().SingleInstance();
            builder.RegisterType<NoDependenciesRepo<AccountCompanyRoleEntity>>().AsSelf().As<IRepo<AccountCompanyRoleEntity>>().SingleInstance();

            builder.RegisterType<NoDependenciesRepo<EnterPointEntity>>().AsSelf().As<IRepo<EnterPointEntity>>().SingleInstance();
            builder.RegisterType<NoDependenciesRepo<CheckPointEntity>>().AsSelf().As<IRepo<CheckPointEntity>>().SingleInstance();
            builder.RegisterType<NoDependenciesRepo<InteractionPointEntity>>().AsSelf().As<IRepo<InteractionPointEntity>>().SingleInstance();
            builder.RegisterType<NoDependenciesRepo<ControlPointEntity>>().AsSelf().As<IRepo<ControlPointEntity>>().SingleInstance();

            return builder.Build();
        }
    }
}

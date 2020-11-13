using Autofac;

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

            return builder.Build();
        }
    }
}

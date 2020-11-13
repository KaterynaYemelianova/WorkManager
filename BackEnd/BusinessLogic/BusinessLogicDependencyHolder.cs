using Autofac;

using BusinessLogic.ServiceContracts;
using BusinessLogic.Services;

namespace BusinessLogic
{
    public static class BusinessLogicDependencyHolder
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

            builder.RegisterType<AuthService>().As<IAuthService>().SingleInstance();

            return builder.Build();
        }
    }
}

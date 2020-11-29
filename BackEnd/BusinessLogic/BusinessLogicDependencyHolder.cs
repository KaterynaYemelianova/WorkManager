using Autofac;

using BusinessLogic.Services;
using BusinessLogic.Services.PointServices;

using BusinessLogic.ServiceContracts;
using BusinessLogic.ServiceContracts.PointServiceContracts;

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
            builder.RegisterType<RSAService>().As<IAsymmetricEncryptionService>().SingleInstance();
            builder.RegisterType<SHA256HashingService>().As<IHashingService>().SingleInstance();
            builder.RegisterType<SessionService>().As<ISessionService>().SingleInstance();
            builder.RegisterType<RoleCheckService>().As<IRoleCheckService>().SingleInstance();
            builder.RegisterType<CompanyService>().As<ICompanyService>().SingleInstance();

            builder.RegisterType<EnterPointService>().As<IEnterPointService>().SingleInstance();
            builder.RegisterType<CheckPointService>().As<ICheckPointService>().SingleInstance();
            builder.RegisterType<InteractionPointService>().As<IInteractionPointService>().SingleInstance();
            builder.RegisterType<ControlPointService>().As<IControlPointService>().SingleInstance();

            builder.RegisterType<MemberLocationService>().As<IMemberLocationService>().AsSelf().SingleInstance();
            builder.RegisterType<ConditionParseService>().As<IConditionParseService>().AsSelf().SingleInstance();
            builder.RegisterType<PointExternalApiService>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}

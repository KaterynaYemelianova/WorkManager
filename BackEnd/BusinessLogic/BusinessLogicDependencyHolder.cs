using Autofac;

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

            //TODO

            return builder.Build();
        }
    }
}

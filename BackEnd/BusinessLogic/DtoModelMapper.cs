using AutoMapper;

using BusinessLogic.Models;

using Dtos;

namespace BusinessLogic
{
    public static class DtoModelMapper
    {
        private static Mapper mapper;
        public static Mapper Mapper
        {
            get
            {
                if (mapper == null)
                    mapper = new Mapper(new MapperConfiguration(cnf => Configure(cnf)));
                return mapper;
            }
        }

        private static void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<PublicKeyDto, PublicKeyModel>();
            config.CreateMap<SignUpDto, AccountModel>()
                  .ForMember(model => model.FirstName, cnf => cnf.AddTransform(name => Util.Capitalize(name)))
                  .ForMember(model => model.LastName, cnf => cnf.AddTransform(name => Util.Capitalize(name)));
        }
    }
}

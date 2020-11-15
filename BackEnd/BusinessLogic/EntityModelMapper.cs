using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Entities;

namespace BusinessLogic
{
    public static class EntityModelMapper
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
            config.CreateMap<AccountModel, AccountEntity>()
                  .ForMember(ent => ent.Password, cnf => cnf.MapFrom(model => model.PasswordHash))
                  .ReverseMap();
        }
    }
}

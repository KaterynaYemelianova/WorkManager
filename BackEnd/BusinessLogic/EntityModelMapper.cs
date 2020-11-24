using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Entities;
using Newtonsoft.Json;

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

            config.CreateMap<RoleModel, RoomEntity>().ReverseMap();

            config.CreateMap<RoomModel, RoomEntity>()
                  .ForMember(ent => ent.ExtraData, cnf => cnf.MapFrom(model => JsonConvert.SerializeObject(model.ExtraData)))
                  .ReverseMap()
                  .ForMember(model => model.ExtraData, cnf => cnf.MapFrom(ent => JsonConvert.DeserializeObject(ent.ExtraData)));

            config.CreateMap<CompanyModel, CompanyEntity>()
                  .ForMember(ent => ent.ExtraData, cnf => cnf.MapFrom(model => JsonConvert.SerializeObject(model.ExtraData)))
                  .ReverseMap()
                  .ForMember(model => model.ExtraData, cnf => cnf.MapFrom(ent => JsonConvert.DeserializeObject(ent.ExtraData)));
        }
    }
}

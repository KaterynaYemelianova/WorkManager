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

            config.CreateMap<AccountRoleDto, AccountRoleModel>()
                  .ForPath(model => model.Account.Id, cnf => cnf.MapFrom(dto => dto.AccountId))
                  .ForPath(model => model.Role.Id, cnf => cnf.MapFrom(dto => dto.RoleId));

            config.CreateMap<CheckPointDto, CheckPointModel>();
            config.CreateMap<EnterPointDto, EnterPointModel>();
            config.CreateMap<InteractionPointDto, InteractionPointModel>();
            config.CreateMap<ControlPointDto, ControlPointModel>();

            config.CreateMap<RoomDto, RoomModel>();
            config.CreateMap<CompanyDto, CompanyModel>();
        }
    }
}

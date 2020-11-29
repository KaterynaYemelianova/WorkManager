using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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

            config.CreateMap<RoleModel, RoleEntity>().ReverseMap();

            CreateMapForPoint<ControlPointModel, ControlPointEntity>(config);
            CreateMapForPoint<EnterPointModel, EnterPointEntity>(config);
            CreateMapForPoint<InteractionPointModel, InteractionPointEntity>(config);
            CreateMapForPoint<CheckPointModel, CheckPointEntity>(config);

            config.CreateMap<RoomModel, RoomEntity>()
                  .ForMember(ent => ent.ExtraData, cnf => cnf.MapFrom(model => JsonConvert.SerializeObject(model.ExtraData)))
                  .ReverseMap()
                  .ForMember(model => model.ExtraData, cnf => cnf.MapFrom(ent => JsonConvert.DeserializeObject(ent.ExtraData)));

            config.CreateMap<CompanyModel, CompanyEntity>()
                  .ForMember(ent => ent.Members, cnf => cnf.ConvertUsing<ARCollection2DictionaryConverter, IEnumerable<AccountRoleModel>>(model => model.Members))
                  .ForMember(ent => ent.ExtraData, cnf => cnf.MapFrom(model => JsonConvert.SerializeObject(model.ExtraData)))
                  .ReverseMap()
                  .ForMember(model => model.Members, cnf => cnf.ConvertUsing<ARDictionary2CollectionConverter, IDictionary<AccountEntity, RoleEntity>>(ent => ent.Members))
                  .ForMember(model => model.ExtraData, cnf => cnf.MapFrom(ent => JsonConvert.DeserializeObject(ent.ExtraData)));
        }

        private class ARCollection2DictionaryConverter : IValueConverter<IEnumerable<AccountRoleModel>, IDictionary<AccountEntity, RoleEntity>>
        {
            public IDictionary<AccountEntity, RoleEntity> Convert(IEnumerable<AccountRoleModel> sourceMember, ResolutionContext context)
            {
                return sourceMember.ToDictionary(
                    ar => Mapper.Map<AccountEntity>(ar.Account),
                    ar => Mapper.Map<RoleEntity>(ar.Role)
                );
            }
        }

        private class ARDictionary2CollectionConverter : IValueConverter<IDictionary<AccountEntity, RoleEntity>, IEnumerable<AccountRoleModel>>
        {
            public IEnumerable<AccountRoleModel> Convert(IDictionary<AccountEntity, RoleEntity> sourceMember, ResolutionContext context)
            {
                return sourceMember.Select(
                    ar => new AccountRoleModel
                    {
                        Account = Mapper.Map<AccountModel>(ar.Key),
                        Role = Mapper.Map<RoleModel>(ar.Value)
                    }
                );
            }
        }

        private static void CreateMapForPoint<TModel, TEntity>(IMapperConfigurationExpression config) 
            where TModel : PointBaseModel where TEntity : PointBaseEntity
        {
            config.CreateMap<TModel, TEntity>()
                  .ForMember(ent => ent.ExtraData, cnf => cnf.MapFrom(model => JsonConvert.SerializeObject(model.ExtraData)))
                  .ReverseMap()
                  .ForMember(model => model.ExtraData, cnf => cnf.MapFrom(ent => JsonConvert.DeserializeObject(ent.ExtraData)));
        }
    }
}

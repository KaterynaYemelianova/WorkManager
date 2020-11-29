using Autofac;

using BusinessLogic.Models;
using BusinessLogic.ServiceContracts;

using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using Dtos;

using Exceptions.BusinessLogic;
using Exceptions.Common;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    internal class CompanyService : ServiceBase, ICompanyService
    {
        private ICompanyRepo CompanyRepo = DataAccessDependencyHolder.Dependencies.Resolve<ICompanyRepo>();
        private IRoomRepo RoomRepo = DataAccessDependencyHolder.Dependencies.Resolve<IRoomRepo>();
        private IAccountRepo AccountRepo = DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();
        private IRepo<RoleEntity> RoleRepo = DataAccessDependencyHolder.Dependencies.Resolve<IRepo<RoleEntity>>();

        private IRepo<AccountCompanyRoleEntity> AccountCompanyRoleRepo = 
            DataAccessDependencyHolder.Dependencies.Resolve<IRepo<AccountCompanyRoleEntity>>(); 
        public async Task<IEnumerable<CompanyModel>> GetCompaniesList()
        {
            IEnumerable<CompanyEntity> companyEntities = await CompanyRepo.Get();
            return EntityModelMapper.Mapper.Map<IEnumerable<CompanyEntity>, IEnumerable<CompanyModel>>(
                companyEntities
            );
        }

        public async Task<CompanyModel> GetCompanyById(int id)
        {
            CompanyEntity companyEntity = await CompanyRepo.GetById(id);
            return EntityModelMapper.Mapper.Map<CompanyEntity, CompanyModel>(companyEntity);
        }

        public async Task<CompanyModel> RegisterCompany(AuthorizedDto<CompanyDto> dto)
        {
            SessionService.CheckSession(dto.Session);
            Dictionary<int, List<AccountRoleDto>> memberGroups = dto.Data.Members?.GroupBy(
                mem => mem.RoleId.Value
            ).ToDictionary(
                group => group.Key,
                group => group.ToList()
            );

            if (memberGroups == null)
                throw new ValidationException("The company must have at least one member");

            if (dto.Data.Members.Select(member => member.AccountId).Distinct().Count() != dto.Data.Members.Count())
                throw new ValidationException("An account can not be used twice in the same company");

            if (memberGroups.ContainsKey((int)RoleEnum.SUPERADMIN))
                throw new ValidationException("Superadmin cannot be a member of the company");

            if (!memberGroups.ContainsKey((int)RoleEnum.DIRECTOR))
                throw new ValidationException("The company must have at least one member in the role of director");

            if (!memberGroups[(int)RoleEnum.DIRECTOR].Any(dir => dir.AccountId == dto.Session.UserId))
                throw new ValidationException("Company creator must act as one of its directors");

            CompanyModel companyModel = DtoModelMapper.Mapper.Map<CompanyModel>(dto.Data);
            CompanyEntity companyEntity = EntityModelMapper.Mapper.Map<CompanyEntity>(companyModel);
            CompanyEntity inserted = await CompanyRepo.Insert(companyEntity);
            return EntityModelMapper.Mapper.Map<CompanyModel>(inserted);
        }

        public async Task<CompanyModel> UpdateCompany(AuthorizedDto<CompanyDto> dto)
        {
            int companyId = dto.Data.Id.Value;
            await CheckSessionAndRole(dto.Session, companyId, RoleEnum.DIRECTOR);

            CompanyEntity company = await CompanyRepo.GetById(companyId);

            company.Name = dto.Data.Name;
            company.ExtraData = JsonConvert.SerializeObject(
                dto.Data.ExtraData
            );

            CompanyEntity updated = await CompanyRepo.Update(company);
            return EntityModelMapper.Mapper.Map<CompanyModel>(updated);
        }

        public async Task DeleteCompany(AuthorizedDto<IdDto> dto)
        {
            int companyId = dto.Data.Id.Value;
            await CheckSessionAndRole(dto.Session, companyId, RoleEnum.DIRECTOR);

            await CompanyRepo.Delete(companyId);
        }

        public async Task<CompanyModel> AddRoom(AuthorizedDto<RoomDto> dto)
        {
            if (!dto.Data.CompanyId.HasValue)
                throw new ValidationException("company_id is required");

            int companyId = dto.Data.CompanyId.Value;
            await CheckSessionAndRole(dto.Session, companyId, RoleEnum.DIRECTOR);

            RoomModel roomModel = DtoModelMapper.Mapper.Map<RoomModel>(dto);
            RoomEntity roomEntity = EntityModelMapper.Mapper.Map<RoomEntity>(roomModel);
            
            CompanyEntity company = await CompanyRepo.GetById(companyId);
            company.Rooms.Add(roomEntity);

            CompanyEntity updated = await CompanyRepo.Update(company);
            return EntityModelMapper.Mapper.Map<CompanyModel>(updated);
        }

        public async Task<RoomModel> UpdateRoom(AuthorizedDto<RoomDto> dto)
        {
            RoomEntity room = await RoomRepo.GetById(dto.Data.Id.Value);
            await CheckSessionAndRole(dto.Session, room.CompanyId, RoleEnum.DIRECTOR);

            RoomModel roomModel = DtoModelMapper.Mapper.Map<RoomModel>(dto);
            RoomEntity roomEntity = EntityModelMapper.Mapper.Map<RoomEntity>(roomModel);
            
            RoomEntity updated = await RoomRepo.Update(roomEntity);
            return EntityModelMapper.Mapper.Map<RoomModel>(updated);
        }

        public async Task DeleteRoom(AuthorizedDto<IdDto> dto)
        {
            RoomEntity room = await RoomRepo.GetById(dto.Data.Id.Value);
            await CheckSessionAndRole(dto.Session, room.CompanyId, RoleEnum.DIRECTOR);

            await RoomRepo.Delete(room.Id);
        }

        public async Task AddMember(AuthorizedDto<AccountCompanyRoleDto> dto)
        {
            CheckRoleValidity(dto.Data.RoleId.Value);

            await CheckSessionAndRole(
                dto.Session, dto.Data.CompanyId.Value, 
                RoleCheckService.GetUpcheckingRoles((RoleEnum)dto.Data.RoleId.Value)
            );

            AccountEntity account = await AccountRepo.GetById(dto.Data.AccountId.Value);
            if (account == null)
                throw new AccountNotFoundException();

            CompanyEntity company = await CompanyRepo.GetById(dto.Data.CompanyId.Value);
            if (company.Members.Any(mem => mem.Key.Id == dto.Data.AccountId))
                throw new MembershipDuplicationException();

            RoleEntity role = await RoleRepo.GetById(dto.Data.RoleId.Value);
            company.Members.Add(account, role);
            
            await CompanyRepo.Update(company);
        }

        public async Task UpdateMember(AuthorizedDto<AccountCompanyRoleDto> dto)
        {
            CheckRoleValidity(dto.Data.RoleId.Value);
            AccountCompanyRoleEntity accountCompanyRole = await AccountCompanyRoleRepo.GetById(dto.Data.Id.Value);

            if (accountCompanyRole == null)
                throw new MembershipNotFoundException();

            if(accountCompanyRole.AccountId != dto.Data.AccountId.Value)
                throw new MembershipNotFoundException();

            if(accountCompanyRole.CompanyId != dto.Data.CompanyId)
                throw new MembershipNotFoundException();

            await CheckSessionAndRole(
                dto.Session, dto.Data.CompanyId.Value,
                RoleCheckService.GetUpcheckingRoles((RoleEnum)accountCompanyRole.RoleId)
            );

            if (accountCompanyRole.RoleId == (int)RoleEnum.DIRECTOR)
            {
                CompanyEntity company = await CompanyRepo.GetById(accountCompanyRole.CompanyId);
                int directorsCount = company.Members.Count(mem => mem.Value.Id == (int)RoleEnum.DIRECTOR);

                if(directorsCount == 1)
                    throw new ValidationException("The company must have at least one member in the role of director");
            }

            accountCompanyRole.RoleId = dto.Data.RoleId.Value;
            await AccountCompanyRoleRepo.Update(accountCompanyRole);
        }

        public async Task DeleteMember(AuthorizedDto<IdDto> dto)
        {
            AccountCompanyRoleEntity accountCompanyRole = await AccountCompanyRoleRepo.GetById(dto.Data.Id.Value);

            if (accountCompanyRole == null)
                throw new MembershipNotFoundException();

            await CheckSessionAndRole(dto.Session, accountCompanyRole.CompanyId, RoleEnum.DIRECTOR);
            await AccountCompanyRoleRepo.Delete(accountCompanyRole.Id);
        }

        private void CheckRoleValidity(int roleId)
        {
            if (roleId < (int)RoleEnum.WORKER || roleId > (int)RoleEnum.SUPERADMIN)
                throw new ValidationException("role_id must be between 1 and 4");
        }
    }
}

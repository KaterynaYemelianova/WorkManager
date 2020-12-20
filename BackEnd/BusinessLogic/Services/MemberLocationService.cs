using Autofac;

using BusinessLogic.Models;
using BusinessLogic.Models.LocationData;
using BusinessLogic.ServiceContracts;

using Dtos;

using Exceptions.BusinessLogic;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    internal class MemberLocationService : IMemberLocationService
    {
        public Dictionary<int, CompanyLocator> Locator = new Dictionary<int, CompanyLocator>();

        private SessionRoleCheckService SessionRoleCheckService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<SessionRoleCheckService>();

        private ICompanyService CompanyService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<ICompanyService>();

        public MemberLocationService()
        {
            Init();
        }

        private async void Init()
        {
            IEnumerable<CompanyModel> companies = await CompanyService.GetCompaniesList();
            foreach (CompanyModel company in companies)
                Locator.Add(company.Id, new CompanyLocator(company));
        }

        public async Task<ICollection<RoomLocator>> GetLocationData(AuthorizedDto<IdDto> dto)
        {
            await SessionRoleCheckService.CheckSessionAndRole(dto.Session, dto.Data.Id.Value, RoleEnum.DIRECTOR);
            if (!Locator.ContainsKey(dto.Data.Id.Value))
                return new List<RoomLocator>();
            return Locator[dto.Data.Id.Value].Values;
        }

        public void NotifyCheckOutAction(AccountModel member, RoomModel roomFrom, RoomModel roomTo)
        {
            NotifyLeaveAction(member, roomFrom);
            NotifyEnterAction(member, roomTo);
        }

        public void NotifyEnterAction(AccountModel member, RoomModel room)
        {
            if (Locator.Values.Any(comp => comp.Values.Any(r => r.Members.ContainsKey(member.Id))))
                throw new AlreadyInsideException();
            Locator[room.CompanyId][room.Id].Members.Add(member.Id, member);
        }

        public void NotifyLeaveAction(AccountModel member, RoomModel room)
        {
            if (!Locator[room.CompanyId][room.Id].Members.ContainsKey(member.Id))
                throw new NotInsideException();
            Locator[room.CompanyId][room.Id].Members.Remove(member.Id);
        }
    }
}

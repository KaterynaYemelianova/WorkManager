using BusinessLogic.Models;
using BusinessLogic.Models.LocationData;
using BusinessLogic.ServiceContracts;

using System.Collections.Generic;

namespace BusinessLogic.Services
{
    internal class MemberLocationService : IMemberLocationService
    {
        private static Dictionary<int, CompanyLocator> Locator = new Dictionary<int, CompanyLocator>();

        public void NotifyCheckOutAction(AccountModel member, RoomModel roomFrom, RoomModel roomTo)
        {
            NotifyLeaveAction(member, roomFrom);
            NotifyEnterAction(member, roomTo);
        }

        public void NotifyEnterAction(AccountModel member, RoomModel room)
        {
            if (!Locator[room.CompanyId][room.Id].Members.ContainsKey(member.Id))
                Locator[room.CompanyId][room.Id].Members.Add(member.Id, member);
            //throw if not
        }

        public void NotifyLeaveAction(AccountModel member, RoomModel room)
        {
            if (Locator[room.CompanyId][room.Id].Members.ContainsKey(member.Id))
                Locator[room.CompanyId][room.Id].Members.Remove(member.Id);
            //throw if not
        }
    }
}

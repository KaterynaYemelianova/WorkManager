using BusinessLogic.Models;
using BusinessLogic.Models.LocationData;
using Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    public interface IMemberLocationService
    {
        void NotifyEnterAction(AccountModel member, RoomModel room);
        void NotifyLeaveAction(AccountModel member, RoomModel room);
        void NotifyCheckOutAction(AccountModel member, RoomModel roomFrom, RoomModel roomTo);

        Task<ICollection<RoomLocator>> GetLocationData(AuthorizedDto<IdDto> dto);
    }
}

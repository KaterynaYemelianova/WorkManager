using BusinessLogic.Models;

namespace BusinessLogic.ServiceContracts
{
    public interface IMemberLocationService
    {
        void NotifyEnterAction(AccountModel member, RoomModel room);
        void NotifyLeaveAction(AccountModel member, RoomModel room);
        void NotifyCheckOutAction(AccountModel member, RoomModel roomFrom, RoomModel roomTo);
    }
}

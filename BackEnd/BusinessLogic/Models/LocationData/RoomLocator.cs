using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Models.LocationData
{
    public class RoomLocator
    {
        public RoomModel Room { get; private set; }
        public Dictionary<int, AccountModel> Members { get; private set; } = new Dictionary<int, AccountModel>();
        
        public RoomLocator(RoomModel room)
        {
            Room = room;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Models.LocationData
{
    public class RoomLocator
    {
        public RoomModel Room { get; private set; }
        
        [JsonIgnore]
        public Dictionary<int, AccountModel> Members { get; private set; } = new Dictionary<int, AccountModel>();

        [JsonProperty("members_list")]
        public ICollection<AccountModel> MembersList => Members.Values;
        
        public RoomLocator(RoomModel room)
        {
            Room = room;
        }
    }
}

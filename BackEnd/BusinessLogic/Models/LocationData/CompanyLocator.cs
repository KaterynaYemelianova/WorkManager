using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Models.LocationData
{
    public class CompanyLocator : Dictionary<int, RoomLocator>
    {
        public CompanyModel Company { get; private set; }

        public CompanyLocator(CompanyModel company)
        {
            Company = company;
            foreach (RoomModel room in company.Rooms)
                Add(room.Id, new RoomLocator(room));
        }
    }
}

using System;
using System.Collections.Generic;

namespace BusinessLogic.Models.Data
{
    public class DetectorData : Dictionary<DateTime, object>
    {
        public void AddData(object data)
        {
            Add(DateTime.Now, data);
        }
    }
}

using System.Collections.Generic;

namespace BusinessLogic.Models.Data
{
    public class PointData : Dictionary<int, DetectorData>
    {
        public void AddData(int detectorId, object data)
        {
            if (!ContainsKey(detectorId))
                Add(detectorId, new DetectorData());
            this[detectorId].AddData(data);
        }
    }
}

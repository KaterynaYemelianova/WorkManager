namespace BusinessLogic.ServiceContracts.PointServiceContracts
{
    public interface IPointService
    {
        void AddData(int pointId, int detectorId, object data);
    }
}

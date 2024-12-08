using TransitNE.Models;

namespace TransitNE.Controllers.Interfaces
{
public interface ISeptaApiService
{
    Task<List<TrainModel>> GetTrainDataAsync();
    Task<List<RailScheduleModel>> GetRailSchedulesAsync(string trainNumber);
    Task<List<StopModel>> GetStopsAsync(string routeNumber);
    Task<List<BusTrolleySchedule>> GetBusTrolleySchedulesAsync(int stopId);
}
}

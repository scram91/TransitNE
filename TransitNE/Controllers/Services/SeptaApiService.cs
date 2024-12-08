using Newtonsoft.Json;
using TransitNE.Controllers.Interfaces;
using TransitNE.Models;

namespace TransitNE.Controllers.Services
{
public class SeptaApiService : ISeptaApiService
{
    private readonly HttpClient _httpClient;

    public SeptaApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Set BaseAddress and default headers if needed.
        // e.g.:
        // _httpClient.BaseAddress = new Uri("https://www3.septa.org/api/");
    }

    public async Task<List<TrainModel>> GetTrainDataAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("TrainView/index.php");
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();
        var trains = JsonConvert.DeserializeObject<List<TrainModel>>(data);
        return trains ?? new List<TrainModel>();
    }

    public async Task<List<RailScheduleModel>> GetRailSchedulesAsync(string trainNumber)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"RRSchedules/index.php?req1={trainNumber}");
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();
        var schedules = JsonConvert.DeserializeObject<List<RailScheduleModel>>(data);
        return schedules ?? new List<RailScheduleModel>();
    }

    public async Task<List<StopModel>> GetStopsAsync(string routeNumber)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"Stops/index.php?req1={routeNumber}");
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();
        var stops = JsonConvert.DeserializeObject<List<StopModel>>(data);
        return stops ?? new List<StopModel>();
    }

    public async Task<List<BusTrolleySchedule>> GetBusTrolleySchedulesAsync(int stopId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"BusSchedules/index.php?stop_id={stopId}");
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();
        var schedules = JsonConvert.DeserializeObject<List<BusTrolleySchedule>>(data);
        return schedules ?? new List<BusTrolleySchedule>();
    }
}

}

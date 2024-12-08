using Microsoft.AspNetCore.Mvc;
using TransitNE.Data;
using TransitNE.Models;
using System.Linq;
using TransitNE.Controllers.Interfaces;

namespace TransitNE.Controllers
{
    public class SeptaRouteInformationController : Controller
    {
        private readonly TransitNEContext _context;
        private readonly ISeptaApiService _apiService;

        public SeptaRouteInformationController(TransitNEContext context, ISeptaApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public IActionResult Septa()
        {
            // If you had logic here previously, it remains. 
            // External API calls should be done via _apiService now.
            return View();
        }

        [HttpGet]
        public void SetTrainModels()
        {
            // Clear existing records
            _context.TrainModel.RemoveRange(_context.TrainModel);

            // Fetch data from the API service
            var trains = _apiService.GetTrainDataAsync().Result;

            // Add the retrieved train data to the database
            foreach (var item in trains)
            {
                _context.TrainModel.Add(item);
            }

            _context.SaveChanges();
        }

        [HttpGet]
        public List<RailScheduleModel> GetSeptaRailSchedules(List<string> trainNo)
        {
            // Clear existing schedules
            _context.RailScheduleModels.RemoveRange(_context.RailScheduleModels);

            List<RailScheduleModel> allSchedules = new List<RailScheduleModel>();

            foreach (var number in trainNo)
            {
                var schedules = _apiService.GetRailSchedulesAsync(number).Result;
                
                // Store the retrieved schedules in the database
                foreach (var sched in schedules)
                {
                    RailScheduleModel railSchedule = new RailScheduleModel
                    {
                        ID = sched.ID,
                        station = sched.station,
                        sched_tm = sched.sched_tm,
                        est_tm = sched.est_tm,
                        act_tm = sched.act_tm
                    };
                    _context.RailScheduleModels.Add(railSchedule);
                }

                _context.SaveChanges();
                allSchedules.AddRange(schedules);
            }

            return allSchedules;
        }

        public List<string> GetSeptaTrainNumbers(string selectedLine)
        {
            // Query the in-memory TrainModel data
            var lines = _context.TrainModel.Where(s => s.line.Contains(selectedLine)).ToList();

            List<string> trainNo = new List<string>();
            foreach (var item in lines)
            {
                trainNo.Add(item.trainno);
            }

            return trainNo;
        }

        [HttpPost]
        public IActionResult GetSelectedLine()
        {
            string selectedLine = Request.Form["RegionalRailLine"].ToString();

            // Refresh train data from API
            SetTrainModels();

            // Retrieve train numbers for the selected line
            List<string> trainNo = GetSeptaTrainNumbers(selectedLine);

            // Retrieve rail schedules for those trains
            List<RailScheduleModel> railSchedules = GetSeptaRailSchedules(trainNo);

            return View(railSchedules);
        }

        [HttpGet]
        public List<StopModel> GetStopInformation(string routeNumber)
        {
            // Clear existing stop data
            _context.StopModels.RemoveRange(_context.StopModels);

            var stops = _apiService.GetStopsAsync(routeNumber).Result;

            // Store stop data in the database
            foreach (var item in stops)
            {
                _context.StopModels.Add(item);
            }

            _context.SaveChanges();

            return stops;
        }

        [HttpGet]
        public List<BusTrolleySchedule> GetBusTrolleySchedule(int stopId)
        {
            // Clear existing schedules
            _context.BusTrolleySchedules.RemoveRange(_context.BusTrolleySchedules);

            var busTrolleySchedules = _apiService.GetBusTrolleySchedulesAsync(stopId).Result;

            // Store schedules in the database
            foreach (var sched in busTrolleySchedules)
            {
                _context.BusTrolleySchedules.Add(sched);
            }

            _context.SaveChanges();

            return busTrolleySchedules;
        }

        [HttpPost]
        public IActionResult GetSelectedBus(string route, string StopName)
        {
            // Retrieve stop information for the given route
            GetStopInformation(route);

            // Get the stop ID for the given stop name
            int lineNumber = GetStopId(StopName);

            // Retrieve bus/trolley schedules for that stop
            List<BusTrolleySchedule> busTrolleys = GetBusTrolleySchedule(lineNumber);

            return View(busTrolleys);
        }

        public int GetStopId(string stopName)
        {
            var busline = _context.StopModels
                                    .Where(m => m.StopName == stopName)
                                    .Select(m => m.StopId).FirstOrDefault();

            string lineNumber = busline.ToString();
            int line = 0;
            line = Int32.Parse(lineNumber);

            return line;
                       
        }
    }
}

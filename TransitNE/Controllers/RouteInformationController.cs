using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.Collections.Immutable;
using System.Drawing.Text;
using System.Text.Encodings.Web;
using TransitNE.Data;
using TransitNE.Models;

namespace TransitNE.Controllers
{
    public class RouteInformationController : Controller
    {
        private readonly TransitNEContext _context;
        Uri address = new("https://www3.septa.org/api/");
        private readonly HttpClient _httpClient;

        public RouteInformationController(TransitNEContext context)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = address;
            _context = context;
        }

        [HttpGet]
        public IActionResult Septa()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            List<TrainModel> trains = new List<TrainModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "TrainView/index.php").Result;


            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                trains = JsonConvert.DeserializeObject<List<TrainModel>>(data)!;
            }

            foreach (var item in trains)
            {
                TrainModel model = new TrainModel
                {
                    ID = item.ID,
                    lat = item.lat,
                    lon = item.lon,
                    trainno = item.trainno,
                    service = item.service,
                    dest = item.dest,
                    currentstop = item.currentstop,
                    nextstop = item.nextstop,
                    line = item.line,
                    consist = item.consist,
                    heading = item.heading,
                    late = item.late,
                    SOURCE = item.SOURCE,
                    TRACK = item.TRACK,
                    TRACK_CHANGE = item.TRACK_CHANGE
                };
                if (_context.TrainModel.IsNullOrEmpty())
                {
                    _context.Add(model);
                }
                else
                {
                    _context.Update(model);
                }
                _context.SaveChanges();
            }
            List<string> trainNo = GetTrainNumbers();
            List<RailScheduleModel> railSchedules = GetRailSchedules(trainNo);


            return View(railSchedules);
        }

        private List<RailScheduleModel> GetRailSchedules(List<string> trainNo)
        {
            _context.RailScheduleModels.RemoveRange();
            List<RailScheduleModel> schedules = new List<RailScheduleModel>();
            for (int i = 0; i < trainNo.Count; i++)
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage scheduleResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "RRSchedules/index.php?req1=" + trainNo[i]).Result;

                if (scheduleResponse.IsSuccessStatusCode)
                {
                    string data = scheduleResponse.Content.ReadAsStringAsync().Result;
                    schedules = JsonConvert.DeserializeObject<List<RailScheduleModel>>(data)!;
                }
                foreach (var item in schedules)
                {
                    RailScheduleModel railSchedule = new RailScheduleModel
                    {
                        ID = item.ID,
                        station = item.station,
                        sched_tm = item.sched_tm,
                        est_tm = item.est_tm,
                        act_tm = item.est_tm,
                    };
                    _context.Add(railSchedule);
                    _context.SaveChanges();
                }
            }
            return schedules;
        }

        public List<string> GetTrainNumbers()
        {
            var result = _context.TrainModel.ToList();

            List<string> trainNo = [];

            foreach (var item in result)
            {
                trainNo.Add(item.trainno);
            }

            return trainNo;
        }
        public IActionResult RouteMap()
        {
            return View();
        }
    }
}

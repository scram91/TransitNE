using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.Collections.Immutable;
using System.Drawing.Text;
using System.Linq;
using System.Text.Encodings.Web;
using TransitNE.Data;
using TransitNE.Models;

namespace TransitNE.Controllers
{
    public class SeptaRouteInformationController : Controller
    {
        private readonly TransitNEContext _context;
        Uri address = new("https://www3.septa.org/api/");
        private readonly HttpClient _httpClient;

        public SeptaRouteInformationController(TransitNEContext context)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = address;
            _context = context;
        }

        public IActionResult Septa()
        {
            GetBusTrolleyInformation();

            return View();
        }

        [HttpGet]
        public void SetTrainModels()
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
         }
        [HttpGet]
        private List<RailScheduleModel> GetSeptaRailSchedules(List<string> trainNo)
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

        public List<string> GetSeptaTrainNumbers(string selectedLine)
        {
            var lines = _context.TrainModel.Where(s => s.line.Contains(selectedLine));
            var result = _context.TrainModel.ToList();

            List<string> trainNo = [];

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
            SetTrainModels();
            List<string> trainNo = GetSeptaTrainNumbers(selectedLine);
            List<RailScheduleModel> railSchedules = GetSeptaRailSchedules(trainNo);
            return View(railSchedules);
        }

        private void GetBusTrolleyInformation()
        {
            throw new NotImplementedException();
        }

    }
}

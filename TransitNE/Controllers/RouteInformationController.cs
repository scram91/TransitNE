using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Drawing.Text;
using System.Text.Encodings.Web;
using TransitNE.Data;
using TransitNE.Models;

namespace TransitNE.Controllers
{
    public class RouteInformationController : Controller
    {
        private readonly TransitNEContext _context;

        Uri address = new("https://www3.septa.org/api/TrainView/index.php");
        private readonly HttpClient _httpClient;


        public RouteInformationController(TransitNEContext context)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = address;
            _context = context;
        }

        [HttpGet]
        public IActionResult Septa(int numTimes = 1)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            List<TrainModel> trains = new List<TrainModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress).Result;


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
                _context.Add(model);
                _context.SaveChanges();
            }
            ViewBag.Message = GetTrainSchedule();
            ViewBag.NumTimes = numTimes;
            return View();
        }

        private static string GetTrainSchedule()
        {
            return "Still Implementing";
        }

        public IActionResult NJTransit()
        {
            return View();
        }

        public IActionResult Patco()
        {
            return View();
        }

        public IActionResult RouteMap()
        {
            return View();
        }
    }
}

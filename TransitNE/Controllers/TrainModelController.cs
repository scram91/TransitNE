using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TransitNE.Data;
using TransitNE.Models;

namespace TransitNE.Controllers
{
    public class TrainModelController : Controller
    {
        Uri address = new("https://www3.septa.org/api/TrainView/index.php");
        private readonly HttpClient _httpClient;

        private TransitNEContext _context;

        public TrainModelController(TransitNEContext context)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = address;
            _context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            List<TrainModel> trains = new List<TrainModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                trains = JsonConvert.DeserializeObject<List<TrainModel>>(data)!;
            }
            List<string> number = new List<string>();

            foreach (var item in trains)
            {
                _context.Add(new TrainModel
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
                });
             }

            return View(trains);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TransitNE.Models;

namespace TransitNE.Controllers
{
    public class TrainModelController : Controller
    {
        Uri address = new("https://www3.septa.org/api/TrainView/index.php");
        private readonly HttpClient _httpClient;

        public TrainModelController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = address;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<TrainModel> trains = new List<TrainModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                trains = JsonConvert.DeserializeObject<List<TrainModel>>(data);
            }

            return View();
        }
    }
}

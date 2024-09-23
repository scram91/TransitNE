using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using TransitNE.Data;
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
                number.Add(item.ToString());
            }

            return View(trains);
        }
    }
}

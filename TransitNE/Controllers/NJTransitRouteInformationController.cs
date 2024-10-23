using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using TransitNE.Data;
using TransitNE.Models.NJTransit;

namespace TransitNE.Controllers
{
    public class NJTransitRouteInformationController : Controller
    {
        private readonly TransitNEContext _context;
        private readonly HttpClient _httpClient;

        public NJTransitRouteInformationController(TransitNEContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }
        public IActionResult NJTransit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetRailStations()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://testraildata.njtransit.com/api/TrainData/getStationList"))
                {
                    request.Headers.TryAddWithoutValidation("accept", "text/plain"); 

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new StringContent("638652253871151576"), "token");
                    request.Content = multipartContent; 

                    var response = await httpClient.SendAsync(request);
                }
            }

            return View();
        }
        
    }
}


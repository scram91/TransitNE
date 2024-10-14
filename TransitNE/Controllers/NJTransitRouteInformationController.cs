using Microsoft.AspNetCore.Mvc;
using TransitNE.Data;

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

        public async Task<IActionResult> getRailStationsAsync()
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://testraildata.njtransit.com/api/TrainData/getStationList"))
            {
                request.Headers.TryAddWithoutValidation("accept", "text/plain");

                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent("638642604401827042"), "token");
                request.Content = multipartContent;

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {

                }
            }
            
            return View();
        }
        
    }
}


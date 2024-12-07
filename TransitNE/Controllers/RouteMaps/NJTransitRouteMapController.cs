using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers.RouteMaps
{
    public class NJTransitRouteMapController : Controller
    {
        public IActionResult NJTransitRouteMap()
        {
            return View();
        }
    }
}

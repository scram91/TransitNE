using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers.RouteMaps
{
    public class PatcoRouteMapController : Controller
    {
        public IActionResult PatcoRouteMap()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers
{
    public class RouteMapController : Controller
    {
        public IActionResult RouteMap()
        {
            return View();
        }

    }
}

using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers
{
    public class NJTransitRouteInformationController : Controller
    {
        public IActionResult NJTransit()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers
{
    public class PatcoRouteInformationController : Controller
    {
        public IActionResult Patco()
        {
            return View();
        }
    }
}

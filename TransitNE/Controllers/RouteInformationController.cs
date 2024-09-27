using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace TransitNE.Controllers
{
    public class RouteInformationController : Controller
    {
        public IActionResult Septa()
        {
            return View();
        }

        public IActionResult NJTransit()
        {
            return View();
        }

        public IActionResult Patco()
        {
            return View();
        }

        public IActionResult RouteMap()
        {
            return View();
        }
    }
}

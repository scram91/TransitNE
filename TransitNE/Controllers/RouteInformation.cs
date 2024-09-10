using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace TransitNE.Controllers
{
    public class RouteInformation : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

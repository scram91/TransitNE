using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers
{
    public class MapController : Controller
    {
        public IActionResult Map()
        {
            return View();
        }
    }
}

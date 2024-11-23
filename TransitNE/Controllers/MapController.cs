using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers
{
    public class MapController : Controller
    {
        public IActionResult Map()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Map(FormCollection form)
        {
            ViewBag.Message = "Start Address: " + form["startLocation"];
           // ViewBag.Message = "End Address: " + form["endLocation"];

            return View();
        }
    }
}

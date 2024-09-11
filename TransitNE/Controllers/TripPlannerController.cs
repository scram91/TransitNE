using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers;

public class TripPlannerController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
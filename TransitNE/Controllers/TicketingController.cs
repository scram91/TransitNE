using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers;

public class TicketingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
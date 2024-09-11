using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers;

public class LoginController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}

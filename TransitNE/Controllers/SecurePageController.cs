using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers
{
    public class SecurePageController : Controller
    {
        public IActionResult SecurePage()
        {
            return View();
        }
    }
}

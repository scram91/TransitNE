using Microsoft.AspNetCore.Mvc;

namespace TransitNE.Controllers
{
    public class PatcoRouteInformationController : Controller
    {
        // The PATCO Site does not use an api to GET transit Schedules
        // They instead use a pdf to display schedules to users
        // This controller will be bare and the view will be used to
        // provide users with links to the schedule and PATCO Travel alerts
        public IActionResult Patco()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace TransitNE.Controllers
{
    public class TripPlannerController : Controller
    {
        public IActionResult Map()
        {
            return View();
        }
    }
}

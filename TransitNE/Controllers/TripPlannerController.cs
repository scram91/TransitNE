using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace TransitNE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripPlannerController : Controller
    {
        public IActionResult Map()
        {
            return View();
        }

        [HttpPost("SaveLocation")]
        public IActionResult SaveLocation([FromBody] DirectionModel location)
        {
            if (location == null)
            {
                return BadRequest(new { message = "Invalid location data." });
            }

            // Log or save the location to a database
            Console.WriteLine($"Address: {location.Address}");
            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");

            return Ok(new { message = "Location saved successfully" });
        }

        public class DirectionModel
        {
            public string? Address { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }

}

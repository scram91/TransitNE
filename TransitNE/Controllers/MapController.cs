using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml;

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
            Address adrs = new Address();
            adrs.StreetAddress = form["startLocation"];
            adrs.Geocode();
           // ViewBag.Message = form["startLocation"];
           // ViewBag.Message = "End Address: " + form["endLocation"];
           var longitude = adrs.Long;
           var latitutde = adrs.Lat;
            return View();
        }
    }

    public class Address
    {
        public Address()
        {
        }
        public string StreetAddress { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public void Geocode()
        {
                        //to Read the Stream
            StreamReader sr = null;
            
            //The Google Maps API Either return JSON or XML. We are using XML Here
            //Saving the url of the Google API 
            string url = String.Format("http://maps.googleapis.com/maps/api/geocode/xml?address=" + 
            this.StreetAddress + "&sensor=false");
            
            //to Send the request to Web Client 
            WebClient wc = new WebClient();
            try
            {
                sr = new StreamReader(wc.OpenRead(url));
            }
            catch (Exception ex)
            {
                throw new Exception("The Error Occured"+ ex.Message);
            }
        }
    }
}

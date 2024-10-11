using System.Web.Mvc;

namespace TransitNE.Models
{
    public class BusTrolleyRouteModel
    {
        public int ID { get; set; }
       // public IEnumerable<SelectListItem> RouteNumbers { get; set; }
        public string? RouteNumber { get; set; }
        public string? RouteName { get; set; }
    }
}

using System.Reflection.Metadata;

namespace TransitNE.Models
{
    public class BusTrolleySchedule
    {
        public int ID { get; set; }
        public string? Stopname { get; set; }
        public string? Route { get; set; }
        public string? date { get; set; }
        public string? day { get; set; }
        public string? Direction { get; set; }
        public string? DateCalendar { get; set; }
        public string? DirectionDesc { get; set; }
    }
}

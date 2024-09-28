namespace TransitNE.Models
{
    public class BusTrolleyModel
    {
        public int ID { get; set; }
        public string? Lat { get; set; }
        public string? Lon { get; set; }
        public string? Label { get; set; }
        public string? VehicleID { get; set; }
        public string? BlockID { get; set; }
        public string? Direction { get; set; }
        public string? Destination { get; set; }
        public int? Heading { get; set; }
        public bool Late { get; set; }
        public bool Original_late { get; set; }
        public string? Offeset_sec { get; set; }
        public string? Trip {  get; set; }
        public string? Next_stop_name { get; set; }
        public int Next_stop_sequence { get; set; }
        public string? Estimated_seat_availability { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}

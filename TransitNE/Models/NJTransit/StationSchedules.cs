namespace TransitNE.Models.NJTransit
{
    public class StationSchedules
    {
        public int Id { get; set; }
        public string? SchedDepart {  get; set; }
        public string? Destination { get; set; }
        public string? Track {  get; set; }
        public string? Line { get; set; }
        public string? TrainId { get; set; }
        public string? ConnectingTrainId { get; set; }
        public string? StationPosition { get; set; }
        public string? Direction { get; set; }
        public string? DwellTime { get; set; }
        public string? PermPickup { get; set; }
        public string? PermDropoff { get; set; }
        public string? StopCode { get; set; }
    }
}

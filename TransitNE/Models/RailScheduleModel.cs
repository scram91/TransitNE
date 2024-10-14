namespace TransitNE.Models
{
    public class RailScheduleModel
    {
        public int ID { get; set; }
        public string? station { get; set; }
        public string? sched_tm { get; set; }
        public string? est_tm { get; set; }
        public string? act_tm { get; set; }
    }
}
namespace TransitNE.Models.NJTransit
{
    public class StationMsg
    {
        public int Id { get; set; }
        public string? MSGType { get; set; }
        public string? MSGText { get; set; }
        public string? MsgPubDate { get; set; }
        public string? MsgId { get; set; }
        public string? MsgAgency { get; set; }
        public string? MsgSource { get; set; }
        public string? StationScope { get; set; }
        public string? LineScope { get; set; }
        public string? MsgPubDateUTC { get; set; }
    }
}

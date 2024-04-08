namespace Flight.Model.DTO
{
    public class DocumentFlightDTO:ListDocumentFlight
    {
        public int Id { get; set; }
        public string FlightNo { get; set; } = string.Empty;
        public Dictionary<string, string> Permission { get; set; }=new Dictionary<string, string>();
        public List<ListDocumentFlight> UpdateVersion { get; set; }= new List<ListDocumentFlight>();
    }
}

namespace Flight.Model.DTO
{
    public class ListDocumentFlight
    {
        public int Id { get; set; }
        public string FlightNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DocumentType { get; set; }= string.Empty;
        public DateTime CreateDate { get; set; }
        public string Creator { get; set; }=string.Empty;
        public string Lastversion { get; set; } = string.Empty;
        public string UrlFile { get; set; } = string.Empty;    
    }
}

namespace Flight.Model.DTO
{
    public class DocumentFlightDTO
    {
        public int Id { get; set; }
        public string FlightNo { get; set; } = string.Empty;
        public int  FlightId {get;set;}
        public string Name { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public string Creator { get; set; } = string.Empty;
        public string Version { get; set; }=string.Empty;
        public string UrlFile { get; set; } = string.Empty;
        public List<DocumentFlightPermissionDTO>? Permission { get; set; }=new List<DocumentFlightPermissionDTO>();
        public List<ListDocumentFlight> UpdateVersion { get; set; }= new List<ListDocumentFlight>();
    }
}

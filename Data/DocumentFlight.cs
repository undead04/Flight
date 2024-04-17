namespace Flight.Data
{
    public class DocumentFlight
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string ExtensionFile { get; set; } = string.Empty;
        public int DocumentTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Create_Date { get; set; }
        public string Version { get; set; } = string.Empty;
        public string CreateUserId { get; set; }=string.Empty;
        public virtual Flight? Flight { get; set; }
        public virtual DocumentType? DocumentType { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public  ICollection<DocumentFlightPermission>? DocumentFlightPermissions { get; set; }

       
    }
}

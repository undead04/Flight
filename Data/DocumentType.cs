namespace Flight.Data
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Note { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Create_Date { get; set; }
        public ICollection<DocumentFlight>? DocumentFlights { get; set; } 
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public ICollection<PermissionDocumentType>? PermissionDocuments { get; set; }

    }
}

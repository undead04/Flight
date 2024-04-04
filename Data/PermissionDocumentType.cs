namespace Flight.Data
{
    public class PermissionDocumentType
    {
        public int Id { get; set; }
        public int DocumnetTypeId { get; set; }
        public bool ClaimsType { get; set; }
        public bool ClaimsValue { get; set; }
        public virtual DocumentType? DocumentType { get; set; }
    }
}

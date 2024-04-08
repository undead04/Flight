namespace Flight.Data
{
    public class PermissionDocumentType
    {
        public int Id { get; set; }
        public int DocumnetTypeId { get; set; }
        public string GroupPermissionId{get;set;}=string.Empty;
        public string ClaimsType { get; set; }=string.Empty;
        public string ClaimsValue { get; set; } = string.Empty;
        public virtual DocumentType? DocumentType { get; set; }
        public virtual GroupPermission? GroupPermission{get;set;}
    }
}

namespace Flight.Model
{
    public class PermissionModel
    {
        public string GroupPermissionId { get; set; } = string.Empty;
        public int DocumentTypeId { get; set; }
        public string ClaimsType { get; set; } = string.Empty;
        public string ClaimsValue { get; set; } = string.Empty;
    }
}

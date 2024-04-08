namespace Flight.Model.DTO
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public int DocumnetTypeId { get; set; }
        public string GroupPermissionId { get; set; } = string.Empty;
        public string ClaimsType { get; set; } = string.Empty;
        public string ClaimsValue { get; set; }=string.Empty;
    }
}

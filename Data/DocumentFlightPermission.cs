namespace Flight.Data
{
    public class DocumentFlightPermission
    {
        public int Id { get;set; }
        public int DocumentFlightId { get; set; }
        public string GroupPermissionId { get; set; } = string.Empty;
        public virtual GroupPermission? GroupPermission { get; set; }
        public virtual DocumentFlight? DocumentFlight { get; set; }
    }
}

namespace Flight.Model.DTO
{
    public class GroupPermissionDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Note { get; set; }=string.Empty;
        public string Name { get; set; }=string.Empty;
        public DateTime Create_At { get; set; }
        public int TotalMember { get; set; }
        public string Creator { get; set; }=string.Empty;
    }
}

namespace Flight.Model.DTO
{
    public class DocumentTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime Create_At { get; set; }
        public string Creator { get; set; } = string.Empty;
        public int Permission { get; set;}
    }
}

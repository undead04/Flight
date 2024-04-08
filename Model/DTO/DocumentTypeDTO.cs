namespace Flight.Model.DTO
{
    public class DocumentTypeDTO:DocumentTypeModel
    {
        public int Id { get; set; }
        public DateTime Create_At { get; set; }
        public string Creator { get; set; } = string.Empty;
        public int Permission { get; set;} = 0;
    }
}

namespace Flight.Model
{
    public class DocumentTypeModel
    {
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; }=string.Empty;
        public List<DocumentTypePermisionModel>? Permission { get; set; }
    }
}

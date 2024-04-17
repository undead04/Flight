namespace Flight.Model
{
    public class DocumentFlightModel
    {
        public int FlightId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Note { get; set; }= string.Empty;
        public List<string> GroupPermissionId { get;set; }=new List<string>();
        public IFormFile? documentFile { get; set; }
    }
}

namespace Flight.Model
{
    public class DocumentFlightUserModel
    {
        public IFormFile? DocumentFile { get; set; }
        public int FlightId { get; set; }
        public string DocumentTitle { get; set; } = string.Empty;
        public int DocumetTypeId { get; set; }

    }
}

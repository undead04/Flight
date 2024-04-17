namespace Flight.Model
{
    public class ConfirmDocumentModel
    {
        public IFormFile? Signature { get; set; }
        public int FlightId { get; set; }
    }
}

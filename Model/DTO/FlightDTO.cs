namespace Flight.Model.DTO
{
    public class FlightDTO
    {
        public int Id{get;set;}
        public string FlightNo{get;set;}=string.Empty;
        public string Route{get;set;}=string.Empty;
        public DateTime DepartureDate { get; set; }
        public decimal TotalDocument{get;set;}
        public string UrlSignature { get; set; }=string.Empty;
        public bool IsConfirm { get; set; } = false;
        public int ReturnFile { get; set; }
        public int SendFile { get; set; }
    }
}
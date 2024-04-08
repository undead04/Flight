namespace Flight.Model
{
    public class FlightModel
    {
        public string FlightNo { get; set; }=string.Empty;
        public DateTime DepartureDate { get; set; }
        public int PoinOfLoading { get; set; } 
        public int PoinOfUnLoad { get; set; }
    }
}
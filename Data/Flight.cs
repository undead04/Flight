namespace Flight.Data
{
    public class Flight
    {
        public int Id { get;set; }
        public string FlightNo { get; set; }=string.Empty;
        public DateTime DepartureDate { get; set; }
        public bool IsConfirm { get; set; }=false;
        public int PoinOfLoading { get; set; } 
        public int PoinOfUnLoad { get; set; }
        public string Signature { get; set;} = string.Empty;
        public virtual Route? Route { get; set; }
        public ICollection<DocumentFlight>? DocumentFlight { get; set; }
       
         
    }
}

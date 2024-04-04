namespace Flight.Data
{
    public class Route
    {
        public int Id { get; set; }
        public string CodeRoute { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ICollection<Flight>? Flights { get; set; }

    }
}

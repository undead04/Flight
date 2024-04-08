using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Repository.FlightRepository
{
    public interface IFlightRepository
    {
        Task<int> CreateFlight(FlightModel model);
        Task<FlightDTO?> GetFlight(int Id);
        Task<List<FlightDTO>> GetAllFlight(string? search, DateTime? date,int? categoryId);
        Task UpdateFlight(int Id,FlightModel model);
        Task DeleteFlight(int Id);
        
    }
}
using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Repository.RouteRepository
{
    public interface IRouteRepository
    {
        Task<List<RouteDTO>> GetAllRoute();
        Task<RouteDTO?> GetRoute(int id);
        Task DeleteRoute(int id);
        Task CreateRoute(RouteModel model);
        Task UpdateRoute(int id,RouteModel model);
    }
}

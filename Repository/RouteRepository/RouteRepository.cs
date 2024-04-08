using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace Flight.Repository.RouteRepository
{
    public class RouteRepository : IRouteRepository
    {
        private readonly MyDbContext context;

        public RouteRepository(MyDbContext context) 
        {
            this.context = context;
        }
        public async Task CreateRoute(RouteModel model)
        {
            var route = new Data.Route
            {
                Name=model.Name,
                CodeRoute=model.CodeRoute,
            };
            await context.routes.AddAsync(route);
            await context.SaveChangesAsync();
        }

        public async Task DeleteRoute(int id)
        {
            var route = await context.routes.FirstOrDefaultAsync(ro => ro.Id == id);
            if(route!=null)
            {
                context.Remove(route);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<RouteDTO>> GetAllRoute()
        {
           var routes=await context.routes.ToListAsync();
            return routes.Select(ro => new RouteDTO
            {
                Id=ro.Id,
                CodeRoute=ro.CodeRoute,
                Name=ro.Name,
            }).ToList();
        }

        public async Task<RouteDTO?> GetRoute(int id)
        {
            var route = await context.routes.FirstOrDefaultAsync(ro => ro.Id == id);
            if(route!=null)
            {
                return new RouteDTO
                {
                    Id=route.Id,
                    Name = route.Name,
                    CodeRoute = route.CodeRoute,
                };
            }
            return null;
        }

        public async Task UpdateRoute(int id, RouteModel model)
        {
            var routes = await context.routes.FirstOrDefaultAsync(ro => ro.Id == id);
            if(routes!=null)
            {
                routes.CodeRoute = model.CodeRoute;
                routes.Name = model.Name;
                await context.SaveChangesAsync();
            }
        }
    }
}

using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace Flight.Repository.FlightRepository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly MyDbContext context;

        public FlightRepository(MyDbContext context)
        {
            this.context = context;
        }
        public async Task<int> CreateFlight(FlightModel model)
        {
            var flight=new Data.Flight
            {
                FlightNo=model.FlightNo,
                DepartureDate=model.DepartureDate,
                PoinOfLoading=model.PoinOfLoading,
                PoinOfUnLoad=model.PoinOfUnLoad
                
            };
            await context.flight.AddAsync(flight);
            await context.SaveChangesAsync();
            return flight.Id;
        }

        public async Task DeleteFlight(int Id)
        {
            var flight=await context.flight.FirstOrDefaultAsync(fl=>fl.Id==Id);
            if(flight!=null) 
            {
                context.Remove(flight);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<FlightDTO>> GetAllFlight(string? search, DateTime? date, int? categoryId)
        {
            var flights=context.flight.Include(f=>f.DocumentFlight).AsQueryable();
            if(!string.IsNullOrEmpty(search))
            {
                flights=flights.Where(fl=>fl.DocumentFlight!=null?fl.DocumentFlight.Any(doc=>doc.Name.Contains(search)):false);
            }
            if(date.HasValue) 
            {
                flights=flights.Where(fl=>fl.DepartureDate.Date==date);
            }
            if(categoryId.HasValue)
            {
                flights=flights.Where(fl=>fl.DocumentFlight!=null?fl.DocumentFlight.Any(doc=>doc.DocumentTypeId==categoryId):false);
            }
            var routes=await context.routes.ToListAsync();
            return await flights.Select(fl=>new FlightDTO
            {
                Id=fl.Id,
                FlightNo=fl.FlightNo,
                Route = GetRouteDescription(fl.PoinOfLoading, fl.PoinOfUnLoad, routes),
                DepartureDate =fl.DepartureDate.Date,
                TotalDocument=fl.DocumentFlight!=null?fl.DocumentFlight.Count:0,
            }).ToListAsync();
        }

        public async Task<FlightDTO?> GetFlight(int Id)
        {
           var flight=await context.flight.FirstOrDefaultAsync(fl=>fl.Id==Id);
           var routes=await context.routes.ToListAsync();
           if(flight!=null)
           {
                return new FlightDTO
                {
                    Id=flight.Id,
                    FlightNo=flight.FlightNo,
                    Route = GetRouteDescription(flight.PoinOfLoading, flight.PoinOfUnLoad, routes),
                    DepartureDate =flight.DepartureDate.Date,
                    TotalDocument=flight.DocumentFlight!=null?flight.DocumentFlight.Count:0,
                };
           }
           return null;
        }

        public async Task UpdateFlight(int Id, FlightModel model)
        {
            var flight=await context.flight.FirstOrDefaultAsync(fl=>fl.Id==Id);
            if(flight!=null) 
            {
                flight.FlightNo=model.FlightNo;
                flight.DepartureDate=model.DepartureDate;
                flight.PoinOfLoading=model.PoinOfLoading;
                flight.PoinOfUnLoad=model.PoinOfUnLoad;
                await context.SaveChangesAsync();
            }
        }
        private static string GetRouteDescription(int? pointOfLoading, int? pointOfUnLoad, List<Data.Route> routes)
        {
            if (pointOfLoading.HasValue && pointOfUnLoad.HasValue)
            {
                var loadingRoute = routes.FirstOrDefault(ro => ro.Id == pointOfLoading);
                var unLoadRoute = routes.FirstOrDefault(ro => ro.Id == pointOfUnLoad);
                if (loadingRoute != null && unLoadRoute != null)
                {
                    return $"{loadingRoute.CodeRoute}-{unLoadRoute.CodeRoute}";
                }
            }
            return string.Empty;
        }

        
    }
}

using System.Threading.Tasks;

namespace Flight.Service.PaginationService
{
    public interface IPaginationService
    {
         Task<IQueryable<T>>  Pagination<T>(IQueryable<T> data,int  page, int pageSize);
    }
}

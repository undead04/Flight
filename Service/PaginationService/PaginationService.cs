using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Flight.Service.PaginationService
{
    public class PaginationService : IPaginationService
    {
        public async Task<IQueryable<T>> Pagination<T>(IQueryable<T> data, int page, int pageSize)
        {
            if (data == null ||await data.CountAsync() == 0)
            {
                throw new ArgumentNullException(nameof(data), "Dữ liệu không được null hoặc rỗng");
            }

            int totalItems =await data.CountAsync(); // Thực hiện truy vấn dữ liệu ở đây
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1 || page > totalPages)
            {
                throw new ArgumentOutOfRangeException(nameof(page), "Trang không hợp lệ");
            }

            var items = data.Skip((page - 1) * pageSize).Take(pageSize); // Thực hiện phân trang ở đây
            return items;
        }
    }
}

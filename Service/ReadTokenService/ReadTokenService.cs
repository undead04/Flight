using Flight.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Flight.Service.ReadTokenService
{
    public class ReadTokenService:IReadTokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public ReadTokenService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }
        public async Task<string> ReadJWT()

        {
            var result = string.Empty;
            var UserId = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (result != null)
                {
                    var user = await userManager.FindByEmailAsync(result);
                    UserId = user!.Id;
                }

            }
            return UserId;
        }
    }
}

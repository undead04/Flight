using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Flight.Authorize
{
    public class RoleRequirement : IAuthorizationRequirement
    {
    }
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            // Kiểm tra xem người dùng có ít nhất một vai trò không
            bool test = context.User.HasClaim(c => c.Type == "role");
            if (context.User != null && context.User.Identity.IsAuthenticated && context.User.HasClaim(c => c.Type == ClaimTypes.Role ))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}

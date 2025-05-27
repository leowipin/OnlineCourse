using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineCourse.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Added for DbContext

namespace OnlineCourse.Filters
{
    public class SecurityStampValidationFilter : IAsyncAuthorizationFilter
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext; // Added DbContext

        public SecurityStampValidationFilter(UserManager<User> userManager, ApplicationDbContext dbContext) // Added DbContext
        {
            _userManager = userManager;
            _dbContext = dbContext; // Added DbContext
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Skip validation if the endpoint allows anonymous access
            if (context.ActionDescriptor.EndpointMetadata.OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>().Any())
            {
                return;
            }

            var userPrincipal = context.HttpContext.User;
            if (userPrincipal?.Identity?.IsAuthenticated != true)
            {
                // Not authenticated, standard [Authorize] attribute will handle it or it's an anomaly.
                // This filter is primarily for an *already authenticated* user's security stamp.
                return;
            }

            var userIdString = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier); // 'sub' claim
            if (string.IsNullOrEmpty(userIdString))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var user = await _userManager.FindByIdAsync(userIdString);
            if (user is null)
            {
                context.Result = new UnauthorizedResult(); // User not found
                return;
            }

            var tokenSecurityStamp = userPrincipal.FindFirstValue("ss");
            if (string.IsNullOrEmpty(tokenSecurityStamp))
            {
                // 'ss' claim is missing. This could be an old token or a misconfiguration.
                context.Result = new UnauthorizedResult();
                return;
            }

            if (user.SecurityStamp != tokenSecurityStamp)
            {
                // Security stamps do not match. Invalidate the request.
                context.Result = new UnauthorizedResult();

                // Elimina todos los refresh tokens activos de este usuario
                if (Guid.TryParse(userIdString, out Guid parsedUserId))
                {
                    var userRefreshTokens = await _dbContext.RefreshTokens
                        .Where(rt => rt.UserId == parsedUserId && !rt.IsUsed && rt.ExpiryDate > DateTime.UtcNow)
                        .ToListAsync();

                    if (userRefreshTokens.Count != 0)
                    {
                        _dbContext.RefreshTokens.RemoveRange(userRefreshTokens);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            // If stamps match, the request proceeds.
        }
    }
}

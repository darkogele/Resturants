using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Restaurants.Application.Users;

public interface IUserContext
{
    CurrentUser? CurrentUser();
}

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? CurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            throw new InvalidOperationException("User is not authenticated.");
        }

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            throw new InvalidOperationException("User is not authenticated.");
        }

        var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.FindAll(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        return new CurrentUser(id, email, roles);
    }
}
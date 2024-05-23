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
        var nationality = user.FindFirst(x => x.Type == "Nationality")?.Value;
        var dateOfBirthString = user.FindFirst(x => x.Type == "DateOfBirth")?.Value;
        var dateOfBirth = dateOfBirthString == null
            ? (DateOnly?)null
            : DateOnly.Parse(dateOfBirthString);

        return new CurrentUser(id, email, roles, nationality, dateOfBirth);
    }
}
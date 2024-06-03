using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Domain.Constants;
using System.Security.Claims;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class UserContextTests
{
    [Fact()]
    public void GetCuuentUser_WithAUthenticatedUser_ShouldReturnCurrentUser()
    {
        // Arrange
        var dateOfBirth = new DateOnly(1987, 3, 31);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "test@test.com"),
            new Claim(ClaimTypes.Role, UserRoles.Admin),
            new Claim(ClaimTypes.Role, UserRoles.User),
            new Claim("Nationality", "Macedonian"),
            new Claim("DateOfBirth", dateOfBirth.ToString())
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(user);

        var userContext = new UserContext(httpContextAccessorMock.Object);

        // Act
        var currentUser = userContext.CurrentUser();

        // Assert
        currentUser.Should().NotBeNull();
        currentUser!.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().BeEquivalentTo([UserRoles.Admin, UserRoles.User]);
        currentUser.Nationality.Should().Be("Macedonian");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact()]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

        var userContext = new UserContext(httpContextAccessorMock.Object);

        // Act
        Action action = () => userContext.CurrentUser();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User is not authenticated.");
    }
}
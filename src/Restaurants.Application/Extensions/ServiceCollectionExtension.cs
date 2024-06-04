using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Users;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtension).Assembly;

        services.AddMediatR(ctf => ctf.RegisterServicesFromAssemblies(applicationAssembly));

        services.AddAutoMapper(applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();

        services.AddScoped<IUserContext, UserContext>();

        services.AddHttpContextAccessor();
    }
}
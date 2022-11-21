using BrotalApiTemplate.Core.ObjectMapping;
using BrotalApiTemplate.Core.Services;
using BrotalApiTemplate.Data;
using BrotalApiTemplate.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrotalApiTemplate.Core;

public static class ServiceConfigurator
{
    public static IServiceCollection ConfigureServices(
        this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddValidatorsFromAssemblyContaining(typeof(ServiceConfigurator));

        services.ConfigureDataAccess();
        
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }
}
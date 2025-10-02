
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SN.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection UseInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, opt=>opt.CommandTimeout(3600)));
        return services;
    }

}
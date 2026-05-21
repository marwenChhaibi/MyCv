using MyCv.Application.Common;
using MyCv.Infrastructure.Persistence;
using MyCv.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyCv.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseNpgsql(config.GetConnectionString("Default")));

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddHostedService<DbSeeder>();

        services.AddHttpClient<UrfFilesClient>(client =>
        {
            client.BaseAddress = new Uri(config["UrfFiles:BaseUrl"]!);
            client.DefaultRequestHeaders.Add("X-Api-Key", config["UrfFiles:ApiKey"]);
        });

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transaction.Seeding.Behaviours;
using Transaction.Seeding.Interfaces;

namespace Transaction.Seeding.Extentions;

public static class SeedingExtentions
{
    public static IServiceCollection AddSeeding(this IServiceCollection services)
    {
        services.AddScoped<ISeedingBehaviour, ExcelSeedingBehaviour>();

        return services;
    }

    public static async Task ApplySeedingAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;

        var behaviours = services.GetRequiredService<IEnumerable<ISeedingBehaviour>>();

        foreach (var behaviour in behaviours)
        {
            await behaviour.SeedAsync();
        }
    }
}

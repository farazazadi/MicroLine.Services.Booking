using Mapster;
using MapsterMapper;
using System.Reflection;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.Mapster;

internal static class DependencyInjection
{
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Default.Settings.MapToConstructor = true;

        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}

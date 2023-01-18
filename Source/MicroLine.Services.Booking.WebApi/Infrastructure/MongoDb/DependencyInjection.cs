using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Interceptors;
using System.Reflection;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Configurations;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

public static class DependencyInjection
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, params Assembly[] assemblies)
    {
        Configure(assemblies);

        services
            .AddOptions<MongoDbOptions>()
            .BindConfiguration(MongoDbOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddSingleton<MongoService>()
            .AddSingleton<ISaveChangesInterceptor, AuditingInterceptor>();

        return services;
    }


    private static void Configure(params Assembly[] assemblies)
    {
        if (!assemblies.Any())
            return;

        assemblies
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(typeInfo => typeInfo.GetInterfaces().Contains(typeof(IMongoConfiguration)))
            .ToList()
            .ForEach(typeInfo =>
            {
                var config = Activator.CreateInstance(typeInfo) as IMongoConfiguration;
                config?.Configure();
            });
    }
    
}
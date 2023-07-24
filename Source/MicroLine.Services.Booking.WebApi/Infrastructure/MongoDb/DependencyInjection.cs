using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Interceptors;
using System.Reflection;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Configurations;
using MongoDB.Bson;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

internal static class DependencyInjection
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
            .AddScoped<MongoService>()
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
                try
                {
                    var config = Activator.CreateInstance(typeInfo) as IMongoConfiguration;
                    config?.Configure();
                }
                catch (BsonSerializationException ex)
                    when (ex.Message.StartsWith("There is already a serializer registered"))
                { }
            });

    }

}
using System.Reflection;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.Integration;

internal static class IntegrationExtensions
{
    private static readonly Dictionary<string,Type> CachedIntegrationEventTypes;

    static IntegrationExtensions()
    {
        CachedIntegrationEventTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(IntegrationEvent)) && !type.IsAbstract)
            .ToDictionary(type => type.Name, type => type);
    }
    
    public static Type? GetCorrespondingIntegrationEventType(this string typeName)
    {
        CachedIntegrationEventTypes.TryGetValue(typeName, out var type);

        return type;
    }
}

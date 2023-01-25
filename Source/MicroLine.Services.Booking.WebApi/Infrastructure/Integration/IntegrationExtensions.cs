using System.Reflection;
using MicroLine.Services.Booking.WebApi.Infrastructure.Inbox;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.Integration;

internal static class IntegrationExtensions
{
    private static readonly Dictionary<string, Type> CachedIntegrationEventTypes;

    static IntegrationExtensions()
    {
        CachedIntegrationEventTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(IntegrationEvent)) && !type.IsAbstract)
            .ToDictionary(type => type.Name, type => type);
    }

    public static IntegrationEvent? ToIntegrationEvent(this InboxMessage message)
    {
        var type = GetCorrespondingIntegrationEventType(message.Subject);

        if (type is null)
            return null;

        var integrationEvent = message.Content.ReadFromJson(type);

        return integrationEvent;
    }

    public static Type? GetCorrespondingIntegrationEventType(this string typeName)
    {
        CachedIntegrationEventTypes.TryGetValue(typeName, out var type);

        return type;
    }
}
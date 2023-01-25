namespace MicroLine.Services.Booking.WebApi.Infrastructure.Integration.IncomingEvents.Airline;

internal class AirportCreatedIntegrationEvent : IntegrationEvent
{
    public override string EventName => nameof(AirportCreatedIntegrationEvent);

    public required string Id { get; init; }
    public required string IcaoCode { get; init; }
    public required string IataCode { get; init; }
    public required string Name { get; init; }
    public required BaseUtcOffsetModel BaseUtcOffset { get; init; }
    public required AirportLocationModel Location { get; init; }


    public record BaseUtcOffsetModel(int Hours, int Minutes);
    public record AirportLocationModel(string Country, string Region, string City);

}

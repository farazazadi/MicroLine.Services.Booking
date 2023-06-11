using MediatR;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.WebApi.Common.Integration;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

namespace MicroLine.Services.Booking.WebApi.Features.Flights;


public class FlightScheduledIntegrationEvent : IntegrationEvent
{
    public override string EventName => nameof(FlightScheduledIntegrationEvent);

    public required Guid FlightId { get; init; }
    public required string FlightNumber { get; init; }
    public required AirportModel OriginAirport { get; init; }
    public required AirportModel DestinationAirport { get; init; }
    public required AircraftModel Aircraft { get; init; }
    public required DateTime ScheduledUtcDateTimeOfDeparture { get; init; }
    public required DateTime ScheduledUtcDateTimeOfArrival { get; init; }
    public required TimeSpan EstimatedFlightDuration { get; init; }
    public required FlightPriceModel Prices { get; init; }
    public required Flight.FlightStatus Status { get; init; }


    public record BaseUtcOffsetModel(int Hours, int Minutes);

    public record AirportModel(
        string IataCode,
        string Name,
        BaseUtcOffsetModel BaseUtcOffset,
        string Country,
        string Region,
        string City
    );

    public record AircraftModel(
        string Model,
        int EconomyClassCapacity,
        int BusinessClassCapacity,
        int FirstClassCapacity
    );

    public record FlightPriceModel(
        MoneyModel EconomyClass,
        MoneyModel BusinessClass,
        MoneyModel FirstClass
    );

    public record MoneyModel(
        decimal Amount,
        Money.CurrencyType Currency
    );




    internal class Handler : INotificationHandler<FlightScheduledIntegrationEvent>
    {
        private readonly MongoService _mongoService;

        public Handler(MongoService mongoService)
        {
            _mongoService = mongoService;
        }

        public async Task Handle(FlightScheduledIntegrationEvent integrationEvent, CancellationToken token)
        {
            Id externalId = integrationEvent.FlightId.ToString();
            FlightNumber flightNumber = integrationEvent.FlightNumber;


            int originAirportBaseUtcOffsetHours = integrationEvent.OriginAirport.BaseUtcOffset.Hours;
            int originAirportBaseUtcOffsetMinutes = integrationEvent.OriginAirport.BaseUtcOffset.Minutes;

            Airport originAirport = Airport.Create(
                integrationEvent.OriginAirport.IataCode,
                integrationEvent.OriginAirport.Name,
                BaseUtcOffset.Create(originAirportBaseUtcOffsetHours, originAirportBaseUtcOffsetMinutes),
                integrationEvent.OriginAirport.Country,
                integrationEvent.OriginAirport.Region,
                integrationEvent.OriginAirport.City
                );


            int destinationAirportBaseUtcOffsetHours = integrationEvent.DestinationAirport.BaseUtcOffset.Hours;
            int destinationAirportBaseUtcOffsetMinutes = integrationEvent.DestinationAirport.BaseUtcOffset.Minutes;

            Airport destinationAirport = Airport.Create(
                integrationEvent.DestinationAirport.IataCode,
                integrationEvent.DestinationAirport.Name,
                BaseUtcOffset.Create(destinationAirportBaseUtcOffsetHours, destinationAirportBaseUtcOffsetMinutes),
                integrationEvent.DestinationAirport.Country,
                integrationEvent.DestinationAirport.Region,
                integrationEvent.DestinationAirport.City
                );


            var aircraft = Domain.Flights.Aircraft.Create(
                integrationEvent.Aircraft.Model,
                integrationEvent.Aircraft.EconomyClassCapacity,
                integrationEvent.Aircraft.BusinessClassCapacity,
                integrationEvent.Aircraft.FirstClassCapacity
            );



            FlightPrice flightPrice = FlightPrice.Create(
                Money.Of(integrationEvent.Prices.EconomyClass.Amount, integrationEvent.Prices.EconomyClass.Currency),
                Money.Of(integrationEvent.Prices.BusinessClass.Amount, integrationEvent.Prices.BusinessClass.Currency),
                Money.Of(integrationEvent.Prices.FirstClass.Amount, integrationEvent.Prices.FirstClass.Currency)
            );

            Flight flight = Flight.Create(
                externalId,
                flightNumber,
                originAirport,
                destinationAirport,
                aircraft,
                integrationEvent.ScheduledUtcDateTimeOfDeparture,
                integrationEvent.ScheduledUtcDateTimeOfArrival,
                integrationEvent.EstimatedFlightDuration,
                flightPrice,
                integrationEvent.Status
            );

            _mongoService.Add(flight, token);

            await _mongoService.SaveChangesAsync(token);
        }
    }

}
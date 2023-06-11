using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.WebApi.Features.Flights;

namespace MicroLine.Services.Booking.Tests.Common.Fakes;

public static class FakeFlightScheduledIntegrationEvent
{
    public static FlightScheduledIntegrationEvent NewFake()
    {
        Flight flight = FakeFlight.NewFake();

        var originAirportModel = new FlightScheduledIntegrationEvent.AirportModel(
            flight.OriginAirport.IataCode,
            flight.OriginAirport.Name,

            new FlightScheduledIntegrationEvent.BaseUtcOffsetModel(
                flight.OriginAirport.BaseUtcOffset.Hours,
                flight.OriginAirport.BaseUtcOffset.Minutes),

            flight.OriginAirport.Country,
            flight.OriginAirport.Region,
            flight.OriginAirport.City
        );


        var destinationAirportModel = new FlightScheduledIntegrationEvent.AirportModel(
            flight.DestinationAirport.IataCode,
            flight.DestinationAirport.Name,

            new FlightScheduledIntegrationEvent.BaseUtcOffsetModel(
                flight.DestinationAirport.BaseUtcOffset.Hours,
                flight.DestinationAirport.BaseUtcOffset.Minutes),

            flight.DestinationAirport.Country,
            flight.DestinationAirport.Region,
            flight.DestinationAirport.City
        );


        var aircraftModel = new FlightScheduledIntegrationEvent.AircraftModel(
            flight.Aircraft.Model,
            flight.Aircraft.EconomyClassCapacity,
            flight.Aircraft.BusinessClassCapacity,
            flight.Aircraft.FirstClassCapacity
        );


        var flightPriceModel = new FlightScheduledIntegrationEvent.FlightPriceModel(

            new FlightScheduledIntegrationEvent.MoneyModel(
                flight.Prices.EconomyClass.Amount,
                flight.Prices.EconomyClass.Currency),

            new FlightScheduledIntegrationEvent.MoneyModel(
                flight.Prices.BusinessClass.Amount,
                flight.Prices.BusinessClass.Currency),

            new FlightScheduledIntegrationEvent.MoneyModel(
                flight.Prices.FirstClass.Amount,
                flight.Prices.FirstClass.Currency)
        );



        FlightScheduledIntegrationEvent integrationEvent = new()
        {
            FlightId = Guid.Parse(flight.Id),
            FlightNumber = flight.FlightNumber,

            OriginAirport = originAirportModel,
            DestinationAirport = destinationAirportModel,

            Aircraft = aircraftModel,

            ScheduledUtcDateTimeOfDeparture = flight.ScheduledUtcDateTimeOfDeparture,
            ScheduledUtcDateTimeOfArrival = flight.ScheduledUtcDateTimeOfArrival,
            EstimatedFlightDuration = flight.EstimatedFlightDuration,

            Prices = flightPriceModel,
            Status = flight.Status
        };

        return integrationEvent;
    }
}
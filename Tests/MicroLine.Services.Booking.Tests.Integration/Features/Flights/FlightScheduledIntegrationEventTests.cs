using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Tests.Common.Fakes;
using MicroLine.Services.Booking.Tests.Integration.Common;
using MicroLine.Services.Booking.WebApi.Features.Flights;

namespace MicroLine.Services.Booking.Tests.Integration.Features.Flights;

public class FlightScheduledIntegrationEventTests : IntegrationTestBase
{
    public FlightScheduledIntegrationEventTests(BookingWebApplicationFactory bookingWebApplicationFactory) : base(bookingWebApplicationFactory)
    {
    }


    [Fact]
    public async Task Flight_ShouldBeCreatedAsExpected_WhenFlightScheduledIntegrationEventReceived()
    {
        // Given
        FlightScheduledIntegrationEvent integrationEvent = FakeFlightScheduledIntegrationEvent.NewFake();


        // When
        RabbitMqPublisher.Publish(integrationEvent, AirlineExchangeName);

        Flight flight = await WaitUntilGetAsync<Flight>(f => f.ExternalId == integrationEvent.FlightId.ToString());


        // Then
        flight.FlightNumber.ToString().Should().Be(integrationEvent.FlightNumber);

        flight.OriginAirport.IataCode.Should().Be(integrationEvent.OriginAirport.IataCode);
        flight.OriginAirport.Name.Should().Be(integrationEvent.OriginAirport.Name);
        flight.OriginAirport.BaseUtcOffset.Hours.Should().Be(integrationEvent.OriginAirport.BaseUtcOffset.Hours);
        flight.OriginAirport.BaseUtcOffset.Minutes.Should().Be(integrationEvent.OriginAirport.BaseUtcOffset.Minutes);
        flight.OriginAirport.Country.Should().Be(integrationEvent.OriginAirport.Country);
        flight.OriginAirport.Region.Should().Be(integrationEvent.OriginAirport.Region);
        flight.OriginAirport.City.Should().Be(integrationEvent.OriginAirport.City);

        flight.DestinationAirport.IataCode.Should().Be(integrationEvent.DestinationAirport.IataCode);
        flight.DestinationAirport.Name.Should().Be(integrationEvent.DestinationAirport.Name);
        flight.DestinationAirport.BaseUtcOffset.Hours.Should().Be(integrationEvent.DestinationAirport.BaseUtcOffset.Hours);
        flight.DestinationAirport.BaseUtcOffset.Minutes.Should().Be(integrationEvent.DestinationAirport.BaseUtcOffset.Minutes);
        flight.DestinationAirport.Country.Should().Be(integrationEvent.DestinationAirport.Country);
        flight.DestinationAirport.Region.Should().Be(integrationEvent.DestinationAirport.Region);
        flight.DestinationAirport.City.Should().Be(integrationEvent.DestinationAirport.City);

        flight.Aircraft.Model.Should().Be(integrationEvent.Aircraft.Model);
        flight.Aircraft.EconomyClassCapacity.Should().Be(integrationEvent.Aircraft.EconomyClassCapacity);
        flight.Aircraft.BusinessClassCapacity.Should().Be(integrationEvent.Aircraft.BusinessClassCapacity);
        flight.Aircraft.FirstClassCapacity.Should().Be(integrationEvent.Aircraft.FirstClassCapacity);


        flight.ScheduledUtcDateTimeOfDeparture.RemoveSecondsAndSmallerTimeUnites()
            .Should().Be(integrationEvent.ScheduledUtcDateTimeOfDeparture.RemoveSecondsAndSmallerTimeUnites());
        flight.ScheduledUtcDateTimeOfArrival.RemoveSecondsAndSmallerTimeUnites()
            .Should().Be(integrationEvent.ScheduledUtcDateTimeOfArrival.RemoveSecondsAndSmallerTimeUnites());
        flight.EstimatedFlightDuration.Should().Be(integrationEvent.EstimatedFlightDuration);

        flight.Prices.EconomyClass.Currency.Should().Be(integrationEvent.Prices.EconomyClass.Currency);
        flight.Prices.EconomyClass.Amount.Should().Be(integrationEvent.Prices.EconomyClass.Amount);
        flight.Prices.BusinessClass.Currency.Should().Be(integrationEvent.Prices.BusinessClass.Currency);
        flight.Prices.BusinessClass.Amount.Should().Be(integrationEvent.Prices.BusinessClass.Amount);
        flight.Prices.FirstClass.Currency.Should().Be(integrationEvent.Prices.FirstClass.Currency);
        flight.Prices.FirstClass.Amount.Should().Be(integrationEvent.Prices.FirstClass.Amount);


        flight.Status.Should().Be(integrationEvent.Status);

    }
}
using MicroLine.Services.Booking.Domain.Airports;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Tests.Common.Fakes;
using MicroLine.Services.Booking.Tests.Integration.Common;
using MicroLine.Services.Booking.WebApi.Features.Airports;
using MicroLine.Services.Booking.WebApi.Infrastructure.Integration.IncomingEvents.Airline;

namespace MicroLine.Services.Booking.Tests.Integration.Features.Airports;

public class CreateAirportTests : IntegrationTestBase
{
    public CreateAirportTests(BookingWebApplicationFactory bookingWebApplicationFactory) : base(bookingWebApplicationFactory)
    {
    }


    [Fact]
    public async Task Airport_ShouldBeCreatedAsExpected_WhenRequestIsValid()
    {
        // Given
        var airport = FakeAirport.NewFake();
        var command = Mapper.Map<CreateAirport.Command>(airport);


        // When
        var createdAirportId = await SendAsync(command);


        // Then
        var createdAirport = await FindByIdAsync<Airport>(createdAirportId);

        command.IcaoCode.Should().BeEquivalentTo(createdAirport!.IcaoCode);
        command.IataCode.Should().BeEquivalentTo(createdAirport!.IataCode);
        command.Name.Should().BeEquivalentTo(createdAirport!.Name);

        command.BaseUtcOffset.Hours.Should().Be(createdAirport!.BaseUtcOffset.Hours);
        command.BaseUtcOffset.Minutes.Should().Be(createdAirport!.BaseUtcOffset.Minutes);

        command.Location.Country.Should().Be(createdAirport!.Location.Country);
        command.Location.Region.Should().Be(createdAirport!.Location.Region);
        command.Location.City.Should().Be(createdAirport!.Location.City);

    }


    [Fact]
    public async Task Airport_ShouldNotBeCreated_WhenIcaoCodeAlreadyExist()
    {
        // Given
        var icaoCode = IcaoCode.Create("ABCD");

        var airport1 = FakeAirport.NewFake(icaoCode: icaoCode);

        var command = Mapper.Map<CreateAirport.Command>(airport1);

        await SendAsync(command);


        // When
        var airport2 = FakeAirport.NewFake(icaoCode: icaoCode);

        command = Mapper.Map<CreateAirport.Command>(airport2);

        var createdAirportId = await SendAsync(command);


        // Then
        createdAirportId.Should().BeEmpty();

        var id = $"{airport2.IataCode}-{airport2.Location.City}";

        var result = await FindByIdAsync<Airport>(id);

        result.Should().BeNull();
    }


    [Fact]
    public async Task Airport_ShouldBeCreated_WhenAirportCreatedIntegrationEventReceived()
    {
        // Given
        var integrationEvent = new AirportCreatedIntegrationEvent
        {
            Id = Id.Create(),
            IcaoCode = "FHZO",
            IataCode = "CIN",
            Name = "Ferryview International Airport",
            BaseUtcOffset = new AirportCreatedIntegrationEvent.BaseUtcOffsetModel(-8, 0),
            Location = new AirportCreatedIntegrationEvent.AirportLocationModel("Puerto Rico", "Rhode Island", "Ferryview")
        };

        // When
        RabbitMqPublisher.Publish(integrationEvent, AirlineExchangeName);

        // Then
        var airportId = $"{integrationEvent.IataCode}-{integrationEvent.Location.City}";

        var airport = await WaitUntilGetAsync<Airport>(airport => airport.Id == airportId);

        airport.Id.ToString().Should().Be(airportId);
        airport.IcaoCode.ToString().Should().Be(integrationEvent.IcaoCode);
        airport.IataCode.ToString().Should().Be(integrationEvent.IataCode);
        airport.Name.ToString().Should().Be(integrationEvent.Name);

        airport.BaseUtcOffset.Hours.Should().Be(integrationEvent.BaseUtcOffset.Hours);
        airport.BaseUtcOffset.Minutes.Should().Be(integrationEvent.BaseUtcOffset.Minutes);

        airport.Location.Country.Should().Be(integrationEvent.Location.Country);
        airport.Location.Region.Should().Be(integrationEvent.Location.Region);
        airport.Location.City.Should().Be(integrationEvent.Location.City);
    }
}
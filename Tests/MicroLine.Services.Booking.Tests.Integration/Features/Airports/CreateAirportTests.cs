using MicroLine.Services.Booking.Domain.Airports;
using MicroLine.Services.Booking.Tests.Common.Fakes;
using MicroLine.Services.Booking.Tests.Integration.Common;
using MicroLine.Services.Booking.WebApi.Features.Airports;

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

}
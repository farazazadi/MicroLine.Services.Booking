using MicroLine.Services.Booking.Domain.Airports;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Tests.Unit.Domain.Airports;

public class AirportTests
{
    [Fact]
    public void Airport_ShouldBeCreatedAsExpected()
    {
        // Given
        var icaoCode = IcaoCode.Create("CYYJ");
        var iataCode = IataCode.Create("YYJ");
        var name = AirportName.Create("Victoria International Airport");
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var airportLocation = AirportLocation.Create("Canada", "British Columbia", "Victoria");

        // When
        var airport = Airport.Create(icaoCode, iataCode, name, baseUtcOffset, airportLocation);

        // Then
        airport.Id.ToString().Should().Be($"{iataCode}-{airport.Location.City}");
        airport.DomainEvents.Count.Should().Be(0);
    }


}

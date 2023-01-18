using MicroLine.Services.Booking.Domain.Airports;
using MicroLine.Services.Booking.Domain.Airports.Exceptions;

namespace MicroLine.Services.Booking.Tests.Unit.Domain.Airports;

public class AirportLocationTests
{
    public static TheoryData<string> NullOrEmptyStrings = new()
    {
        "",
        " ",
        "        ",
        string.Empty,
        null
    };

    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndCountryIsNullOrEmpty(string country)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(country, "Ontario", "Toronto");

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndRegionIsNullOrEmpty(string region)
    {
        // Given
        // When
        var action = () => AirportLocation.Create("Canada", region, "Toronto");

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndCityIsNullOrEmpty(string city)
    {
        // Given
        // When
        var action = () => AirportLocation.Create("Canada", "Ontario", city);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }



    [Fact]
    public void AirportLocation_ShouldHaveValidToStringOutput_WhenItCreatedFromValidInput()
    {
        // Given
        var country = "Canada";
        var region = "Ontario";
        var city = "Toronto";

        var expected = $"{country}, {region}, {city}";

        // When
        var airportLocation = AirportLocation.Create(country, region, city);

        // Then
        airportLocation.ToString().Should().Be(expected);

    }

}

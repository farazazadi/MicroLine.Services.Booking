using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Domain.Flights.Exceptions;
using MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

namespace MicroLine.Services.Booking.Tests.Unit.Domain.Flights;

public class AirportTests
{
    [Fact]
    public void Airport_ShouldBeCreatedAsExpected()
    {
        // Given
        var iataCode = " YYJ  ";
        var name = "Victoria International Airport  ";
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var country = "  Canada  ";
        var region = "  British Columbia  ";
        var city = "  Victoria  ";

        // When
        var airport = Airport.Create(iataCode, name, baseUtcOffset, country, region, city);

        // Then
        airport.IataCode.Should().Be(iataCode.Trim().ToUpperInvariant());
        airport.Name.Should().Be(name.Trim());
        airport.BaseUtcOffset.Should().BeEquivalentTo(baseUtcOffset);
        airport.Country.Should().Be(country.Trim());
        airport.Region.Should().Be(region.Trim());
        airport.City.Should().Be(city.Trim());
    }


    public static TheoryData<string?> NullOrEmptyStrings = new()
    {
        " ",
        "        ",
        string.Empty,
        null
    };


    #region IataCodeTests

    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Airport_ShouldThrowInvalidAirportException_WhenIataCodeIsNullOrEmpty(string iataCode)
    {
        // Given
        var name = "Victoria International Airport";
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var country = "Canada";
        var region = "British Columbia";
        var city = "Victoria";

        // When
        Func<Airport> func = () => Airport.Create(iataCode, name, baseUtcOffset, country, region, city);


        // Then
        func.Should().ThrowExactly<InvalidAirportException>()
            .And.Code.Should().Be(nameof(InvalidAirportException));
    }



    public static TheoryData<string> IataCodeStringsWithNonEnglishCharacters = new()
    {
        "YJف",
        "YJ ",
        "YY1",
        "YYφ",
    };

    [Theory, MemberData(nameof(IataCodeStringsWithNonEnglishCharacters))]
    public void Airport_ShouldThrowInvalidAirportException_WhenIataCodeContainsNoneEnglishCharacters(string iataCode)
    {
        // Given
        // When
        Func<Airport> func = () => FakeAirport.NewFake(iataCode: iataCode);


        // Then
        func.Should().ThrowExactly<InvalidAirportException>()
            .And.Code.Should().Be(nameof(InvalidAirportException));
    }



    public static TheoryData<string> IataCodeStringsWithInvalidLength = new()
    {
        "JJ",
        "YYYJ             ",
        "YYJJ",
        "           YY",
    };

    [Theory, MemberData(nameof(IataCodeStringsWithInvalidLength))]
    public void Airport_ShouldThrowInvalidAirportException_WhenLengthOfIataCodeIsLessThanOrGreaterThan3(string iataCode)
    {
        // Given
        // When
        Func<Airport> func = () => FakeAirport.NewFake(iataCode: iataCode);


        // Then
        func.Should().ThrowExactly<InvalidAirportException>()
            .And.Code.Should().Be(nameof(InvalidAirportException));
    }



    #endregion


    #region NameTests

    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Airport_ShouldThrowInvalidAirportException_WhenNameIsNullOrEmpty(string name)
    {
        // Given
        var iataCode = "YYJ";
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var country = "Canada";
        var region = "British Columbia";
        var city = "Victoria";

        // When
        Func<Airport> func = () => Airport.Create(iataCode, name, baseUtcOffset, country, region, city);


        // Then
        func.Should().ThrowExactly<InvalidAirportException>()
            .And.Code.Should().Be(nameof(InvalidAirportException));
    }



    public static TheoryData<string> AirportNameStringsWithInvalidLength = new()
    {
        "a",
        "abc             ",
        "           abc",
        "DfqoZq7TfubyboY3V9wwDuTV15Jvj6KV7ybQy1grargD0ziPg34ekVXzVEJwz",
        "DfqoZq7TfubyboY3V9ww DuTV15Jvj6KV7ybQy1gr argD0ziPg34ekVXzVEJw",
    };

    [Theory, MemberData(nameof(AirportNameStringsWithInvalidLength))]
    public void Airport_ShouldThrowInvalidAirportException_WhenLengthOfNameIsLessThan4OrGreaterTha60(string name)
    {
        // Given
        // When
        Func<Airport> func = () => FakeAirport.NewFake(name: name);


        // Then
        func.Should().ThrowExactly<InvalidAirportException>()
            .And.Code.Should().Be(nameof(InvalidAirportException));
    }



    #endregion


    #region LocationTests

    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Airport_ShouldThrowInvalidAirportException_WhenCountryIsNullOrEmpty(string country)
    {
        // Given
        var iataCode = "YYJ";
        var name = "Victoria International Airport";
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var region = "British Columbia";
        var city = "Victoria";

        // When
        Func<Airport> func = () => Airport.Create(iataCode, name, baseUtcOffset, country, region, city);


        // Then
        func.Should().ThrowExactly<InvalidAirportException>()
            .And.Code.Should().Be(nameof(InvalidAirportException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Airport_ShouldThrowInvalidAirportException_WhenRegionIsNullOrEmpty(string region)
    {
        // Given
        var iataCode = "YYJ";
        var name = "Victoria International Airport";
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var country = "Canada";
        var city = "Victoria";

        // When
        Func<Airport> func = () => Airport.Create(iataCode, name, baseUtcOffset, country, region, city);


        // Then
        func.Should().ThrowExactly<InvalidAirportException>()
            .And.Code.Should().Be(nameof(InvalidAirportException));
    }


   [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Airport_ShouldThrowInvalidAirportException_WhenCityIsNullOrEmpty(string city)
    {
        // Given
        var iataCode = "YYJ";
        var name = "Victoria International Airport  ";
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var country = "Canada";
        var region = "British Columbia";

        // When
        Func<Airport> func = () => Airport.Create(iataCode, name, baseUtcOffset, country, region, city);


        // Then
        func.Should().ThrowExactly<InvalidAirportException>()
            .And.Code.Should().Be(nameof(InvalidAirportException));
    }


    #endregion

}

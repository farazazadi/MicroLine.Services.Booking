
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Domain.Flights.Exceptions;

namespace MicroLine.Services.Booking.Tests.Unit.Domain.Flights;

public class AircraftTests
{

    public static TheoryData<string?> NullOrEmptyStrings = new()
    {
        " ",
        "        ",
        string.Empty,
        null
    };

    #region ModelTests

    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Aircraft_ShouldThrowInvalidAircraftException_WhenModelIsNullOrEmptyString(string model)
    {
        // Given
        int economyClassCapacity = 200;
        int businessClassCapacity = 20;
        int firstClassCapacity = 10;

        // When
        var func = () => Aircraft.Create(model, economyClassCapacity, businessClassCapacity, firstClassCapacity);

        // Then
        func.Should().ThrowExactly<InvalidAircraftException>()
            .And.Code.Should().Be(nameof(InvalidAircraftException));
    }



    public static TheoryData<string> AircraftModelStringsWithLengthLessThan5OrGreaterThan25 = new()
    {
        "323",
        "abcs",
        "a             ",
        "           a",
        " a3  ",
        "     a3200000000000000000000455       ",
        "1"
    };

    [Theory, MemberData(nameof(AircraftModelStringsWithLengthLessThan5OrGreaterThan25))]
    public void Aircraft_ShouldThrowInvalidAircraftException_WhenLengthOfModelIsLessThan5OrGreaterThan25(string model)
    {
        // Given
        int economyClassCapacity = 200;
        int businessClassCapacity = 20;
        int firstClassCapacity = 10;

        // When
        Func<Aircraft> func = () => Aircraft.Create(model, economyClassCapacity, businessClassCapacity, firstClassCapacity);

        // Then
        func.Should().ThrowExactly<InvalidAircraftException>()
            .And.Code.Should().Be(nameof(InvalidAircraftException));
    }


    #endregion


    #region SeatingCapacityTests

    public static TheoryData<int, int, int> InvalidSeatingCapacityList = new()
    {
        {-1, 10, 20},
        {1, -5, 20},
        {1, 5, -20},
        {0, 0, 0}
    };

    [Theory, MemberData(nameof(InvalidSeatingCapacityList))]
    public void Aircraft_ShouldThrowInvalidPassengerSeatingCapacityException_WhenPassengerSeatingCapacitiesAreNotValid(
        int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        // Given
        // When
        Func<Aircraft> func = () => Aircraft.Create("Airbus A380", economyClassCapacity, businessClassCapacity, firstClassCapacity);

        // Then
        func.Should().ThrowExactly<InvalidAircraftException>()
            .And.Code.Should().Be(nameof(InvalidAircraftException));
    }

    #endregion

}
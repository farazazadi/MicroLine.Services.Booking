using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Domain.Flights.Exceptions;
using MicroLine.Services.Booking.Tests.Common.Fakes;

namespace MicroLine.Services.Booking.Tests.Unit.Domain.Flights;

public class FlightTests
{
    [Fact]
    public void Flight_ShouldThrowInvalidScheduledDateTimeOfDeparture_WhenScheduledUtcDateTimeOfDepartureIsInPastTime()
    {
        // Given
        DateTime scheduledUtcDateTimeOfDeparture = DateTime.UtcNow.AddHours(-1);


        // When
        Func<Flight> func = () => FakeFlight.NewFake(scheduledUtcDateTimeOfDeparture: scheduledUtcDateTimeOfDeparture);


        
        // Then
        func.Should().ThrowExactly<InvalidScheduledDateTimeOfDeparture>()
            .And.Code.Should().Be(nameof(InvalidScheduledDateTimeOfDeparture));
    }



    public static TheoryData<DateTime, DateTime> WrongDepartureAndArrivalDateTimes = new()
    {
        // Departure                                                        Arrival
        {DateTime.UtcNow.AddHours(5).RemoveSecondsAndSmallerTimeUnites(), DateTime.UtcNow.AddHours(4).RemoveSecondsAndSmallerTimeUnites()},
        {DateTime.UtcNow.AddHours(2).RemoveSecondsAndSmallerTimeUnites(), DateTime.UtcNow.AddHours(2).RemoveSecondsAndSmallerTimeUnites()},
    };

    
    [Theory, MemberData(nameof(WrongDepartureAndArrivalDateTimes))]
    public void Flight_ShouldThrowInvalidScheduledDateTimeOfDeparture_WhenScheduledUtcDateTimeOfDepartureIsGreaterOrEqualToScheduledUtcDateTimeOfArrival(
        DateTime scheduledUtcDateTimeOfDeparture, DateTime scheduledUtcDateTimeOfArrival)
    {

        // Given
        // When
        Func<Flight> func = () => FakeFlight.NewFake(
            scheduledUtcDateTimeOfDeparture: scheduledUtcDateTimeOfDeparture,
            scheduledUtcDateTimeOfArrival: scheduledUtcDateTimeOfArrival);


        // Then
        func.Should().ThrowExactly<InvalidScheduledDateTimeOfDeparture>()
            .And.Code.Should().Be(nameof(InvalidScheduledDateTimeOfDeparture));
    }

}
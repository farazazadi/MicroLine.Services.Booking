using MicroLine.Services.Booking.Domain.Common.Exceptions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Tests.Common.Fakes;

namespace MicroLine.Services.Booking.Tests.Unit.Domain.Passengers;

public class PassengerTests
{

    [Fact]
    public void Passenger_ShouldNotHaveAnyDomainEvent_WhenCreated()
    {
        // Given
        // When
        Passenger passenger = FakePassenger.NewFake();
        
        // Then
        passenger.DomainEvents.Count.Should().Be(0);
    }

    [Fact]
    public void Passenger_ShouldThrowInvalidIdException_WhenItCreatedWithTransientId()
    {
        // Given
        Id id = Id.Transient;

        // When
        Func<Passenger> func = () => FakePassenger.NewFake(relatedUserExternalId: id);

        // Then
        func.Should().ThrowExactly<InvalidIdException>()
            .And.Code.Should().Be(nameof(InvalidIdException));
    }
}


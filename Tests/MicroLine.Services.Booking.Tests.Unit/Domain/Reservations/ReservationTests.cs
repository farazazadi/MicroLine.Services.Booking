using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Domain.Reservations;
using MicroLine.Services.Booking.Domain.Reservations.Events;
using MicroLine.Services.Booking.Domain.Reservations.Exceptions;
using MicroLine.Services.Booking.Tests.Common.Fakes;

namespace MicroLine.Services.Booking.Tests.Unit.Domain.Reservations;

public class ReservationTests
{

    [Fact]
    public void Reservation_ShouldHoldAsExpected()
    {
        // Given
        Flight flight = FakeFlight.NewFake();

        List<Passenger> passengers = FakePassenger.NewFakeList(2);


        // When
        Reservation reservation = Reservation.Hold(flight, passengers);


        // Then
        reservation.Status.Should().Be(Reservation.ReservationStatus.Held);
        reservation.DomainEvents.Should().ContainSingle(e => e is ReservationHeldEvent);
    }


    [Fact]
    public void Reservation_ShouldThrowInvalidReservationException_WhenHoldWithoutAnyPassenger()
    {
        // Given
        List<Passenger> emptyPassengersList = new();

        // When
        Func<Reservation> func = ()=>
            FakeReservation.Hold(passengers: emptyPassengersList);


        // Then
        func.Should().ThrowExactly<InvalidReservationException>()
            .And.Code.Should().Be(nameof(InvalidReservationException));
    }

}
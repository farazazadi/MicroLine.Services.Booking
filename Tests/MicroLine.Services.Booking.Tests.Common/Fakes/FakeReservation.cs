using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Domain.Reservations;

namespace MicroLine.Services.Booking.Tests.Common.Fakes;

public static class FakeReservation
{
    public static Reservation Hold(Flight? flight = null, List<Passenger>? passengers = null)
    {
        flight ??= FakeFlight.NewFake();

        passengers ??= FakePassenger.NewFakeList(3);

        return Reservation.Hold(flight, passengers);
    }
}
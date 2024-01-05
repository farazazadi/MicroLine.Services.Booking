using MicroLine.Services.Booking.Domain.Common;

namespace MicroLine.Services.Booking.Domain.Reservations.Events;

public class ReservationHeldEvent : DomainEvent
{
    public Reservation Reservation { get; }

    public ReservationHeldEvent(Reservation reservation)
    {
        Reservation = reservation;
    }
}
using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Reservations.Exceptions;

public class InvalidReservationException : DomainException
{
    public override string Code => nameof(InvalidReservationException);

    public InvalidReservationException(string message) : base(message)
    {
    }
}
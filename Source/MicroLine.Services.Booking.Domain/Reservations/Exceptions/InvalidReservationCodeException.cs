using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Reservations.Exceptions;

public class InvalidReservationCodeException : DomainException
{
    public override string Code => nameof(InvalidReservationCodeException);

    public InvalidReservationCodeException(string message) : base(message)
    {
    }
}
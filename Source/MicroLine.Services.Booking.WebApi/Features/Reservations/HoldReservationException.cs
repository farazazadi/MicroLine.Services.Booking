using MicroLine.Services.Booking.WebApi.Common.Exceptions;

namespace MicroLine.Services.Booking.WebApi.Features.Reservations;

internal class HoldReservationException : ApplicationExceptionBase
{
    public override string Code => nameof(HoldReservationException);

    public HoldReservationException(string message) : base(message)
    {
    }
}
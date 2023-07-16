using MicroLine.Services.Booking.WebApi.Common.Exceptions;

namespace MicroLine.Services.Booking.WebApi.Features.Passengers;

internal class CreatePassengerException : ApplicationExceptionBase
{
    public override string Code => nameof(CreatePassengerException);


    public CreatePassengerException(string message) : base(message)
    {
    }
}
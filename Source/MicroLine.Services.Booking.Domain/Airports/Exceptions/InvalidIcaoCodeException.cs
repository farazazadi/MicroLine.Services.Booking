using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Airports.Exceptions;

public class InvalidIcaoCodeException : DomainException
{
    public override string Code => nameof(InvalidIcaoCodeException);

    public InvalidIcaoCodeException(string message) : base(message)
    {
    }

}
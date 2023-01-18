using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Airports.Exceptions;

public class InvalidIataCodeException : DomainException
{
    public override string Code => nameof(InvalidIataCodeException);

    public InvalidIataCodeException(string message) : base(message)
    {
    }

}
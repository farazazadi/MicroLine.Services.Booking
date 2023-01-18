using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Airports.Exceptions;

public class InvalidAirportLocationException : DomainException
{
    public override string Code => nameof(InvalidAirportLocationException);

    public InvalidAirportLocationException(string message) : base(message)
    {
    }

}

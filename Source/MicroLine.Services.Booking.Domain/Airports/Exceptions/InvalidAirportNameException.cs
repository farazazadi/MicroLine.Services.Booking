using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Airports.Exceptions;

public class InvalidAirportNameException : DomainException
{
    public override string Code => nameof(InvalidAirportNameException);

    public InvalidAirportNameException(string message) : base(message)
    {
    }

}


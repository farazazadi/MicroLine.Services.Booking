using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Flights.Exceptions;

public sealed class InvalidAircraftException : DomainException
{
    public override string Code => nameof(InvalidAircraftException);

    public InvalidAircraftException(string message) : base(message)
    {
    }

}
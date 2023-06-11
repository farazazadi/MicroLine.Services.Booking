using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Flights.Exceptions;

public sealed class InvalidAirportException : DomainException
{
    public override string Code => nameof(InvalidAirportException);

    public InvalidAirportException(string message) : base(message)
    {
    }

}
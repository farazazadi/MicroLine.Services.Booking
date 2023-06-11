using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Flights.Exceptions;

public sealed class InvalidFlightNumberException : DomainException
{
    public override string Code => nameof(InvalidFlightNumberException);

    public InvalidFlightNumberException() : base("FlightNumber is not valid!")
    {
    }

    public InvalidFlightNumberException(string message) : base(message)
    {
    }

}
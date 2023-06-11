using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Flights.Exceptions;

public class InvalidFlightPriceException : DomainException
{
    public override string Code => nameof(InvalidFlightPriceException);

    public InvalidFlightPriceException(string message) : base(message)
    {
    }

}
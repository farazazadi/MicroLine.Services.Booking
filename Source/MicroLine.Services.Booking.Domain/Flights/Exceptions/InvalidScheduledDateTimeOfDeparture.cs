using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Flights.Exceptions;

public class InvalidScheduledDateTimeOfDeparture : DomainException
{
    public override string Code => nameof(InvalidScheduledDateTimeOfDeparture);

    public InvalidScheduledDateTimeOfDeparture(string message) : base(message)
    {
    }
}
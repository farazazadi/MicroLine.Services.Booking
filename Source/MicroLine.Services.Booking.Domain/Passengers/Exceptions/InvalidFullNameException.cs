using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Passengers.Exceptions;

public class InvalidFullNameException : DomainException
{
    public override string Code => nameof(InvalidFullNameException);

    public InvalidFullNameException(string message) : base(message)
    {
    }
}
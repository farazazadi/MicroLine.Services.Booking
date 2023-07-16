using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Passengers.Exceptions;

public class InvalidPassportNumberException : DomainException
{
    public override string Code => nameof(InvalidPassportNumberException);

    public InvalidPassportNumberException(string message) : base(message)
    {
    }

}
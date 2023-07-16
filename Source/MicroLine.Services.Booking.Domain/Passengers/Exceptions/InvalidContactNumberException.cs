using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Passengers.Exceptions;

public class InvalidContactNumberException : DomainException
{
    public override string Code => nameof(InvalidContactNumberException);

    public InvalidContactNumberException(string message) : base(message)
    {
    }

}
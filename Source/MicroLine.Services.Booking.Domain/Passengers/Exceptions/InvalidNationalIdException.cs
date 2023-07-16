using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Passengers.Exceptions;
public class InvalidNationalIdException : DomainException
{
    public override string Code => nameof(InvalidNationalIdException);

    public InvalidNationalIdException(string message) : base(message)
    {
    }

}
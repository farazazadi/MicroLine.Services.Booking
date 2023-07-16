using MicroLine.Services.Booking.Domain.Common.Enums;

namespace MicroLine.Services.Booking.Domain.Common.Exceptions;

public class InvalidGenderException : DomainException
{
    public override string Code => nameof(InvalidGenderException);

    public InvalidGenderException(string message) : base(message)
    {
    }

    public InvalidGenderException(Gender gender) : base($"{gender} is not a valid gender!")
    {
    }
}
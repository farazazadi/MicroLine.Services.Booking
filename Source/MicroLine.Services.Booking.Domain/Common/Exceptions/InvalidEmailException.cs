namespace MicroLine.Services.Booking.Domain.Common.Exceptions;

public class InvalidEmailException : DomainException
{
    public override string Code => nameof(InvalidEmailException);

    public InvalidEmailException(string message) : base(message)
    {
    }

}
namespace MicroLine.Services.Booking.Domain.Common.Exceptions;

public sealed class InvalidIdException : DomainException
{
    public override string Code => nameof(InvalidIdException);

    public InvalidIdException(string message) : base(message)
    {
    }

    public InvalidIdException() : base("Id is not valid!")
    {
    }
}
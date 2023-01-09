namespace MicroLine.Services.Booking.Domain.Common.Exceptions;

public sealed class InvalidIdException : DomainException
{
    public override string Code => nameof(InvalidIdException);

    public InvalidIdException(string id) : base($"Invalid Id: {id}")
    {
    }
    
}
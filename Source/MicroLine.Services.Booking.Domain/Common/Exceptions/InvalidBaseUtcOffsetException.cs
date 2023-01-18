namespace MicroLine.Services.Booking.Domain.Common.Exceptions;
public sealed class InvalidBaseUtcOffsetException : DomainException
{
    public override string Code => nameof(InvalidBaseUtcOffsetException);

    public InvalidBaseUtcOffsetException() : base("BaseUtcOffset is not valid!")
    {
    }
}

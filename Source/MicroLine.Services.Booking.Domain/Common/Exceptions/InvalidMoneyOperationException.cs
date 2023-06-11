namespace MicroLine.Services.Booking.Domain.Common.Exceptions;

public class InvalidMoneyOperationException : DomainException
{
    public override string Code => nameof(InvalidMoneyOperationException);

    public InvalidMoneyOperationException(string message) : base(message)
    {
    }
}
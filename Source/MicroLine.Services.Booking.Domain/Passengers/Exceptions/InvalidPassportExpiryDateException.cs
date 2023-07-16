using MicroLine.Services.Booking.Domain.Common.Exceptions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Domain.Passengers.Exceptions;

public class InvalidPassportExpiryDateException : DomainException
{
    public override string Code => nameof(InvalidPassportExpiryDateException);

    public InvalidPassportExpiryDateException(Date expiryDate) : base(
        $"Passport's ExpiryDate ({expiryDate}) is not valid!")
    {
    }
    public InvalidPassportExpiryDateException(string message) : base(message)
    {
    }

}
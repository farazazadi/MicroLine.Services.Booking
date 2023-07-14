using MicroLine.Services.Booking.Domain.Common.Exceptions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Domain.Passengers.Exceptions;

public class InvalidPassportBirthDateException : DomainException
{
    public override string Code => nameof(InvalidPassportBirthDateException);

    public InvalidPassportBirthDateException(Date birthDate) : base(
        $"Passport's BirthDate ({birthDate}) is not valid!")
    {
    }
    public InvalidPassportBirthDateException(string message) : base(message)
    {
    }

}
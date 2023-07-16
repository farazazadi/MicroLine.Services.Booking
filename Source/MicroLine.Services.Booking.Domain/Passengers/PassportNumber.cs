using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Passengers.Exceptions;

namespace MicroLine.Services.Booking.Domain.Passengers;

public sealed class PassportNumber : ValueObject
{
    private readonly string _passportNumber;

    private PassportNumber(string passportNumber)
    {
        _passportNumber = passportNumber;
    }

    public static PassportNumber Create(string passportNumber)
    {
        Validate(passportNumber);

        return new PassportNumber(passportNumber.Trim());
    }

    private static void Validate(string passportNumber)
    {
        if (passportNumber.IsNullOrWhiteSpace())
            throw new InvalidPassportNumberException("Passport number can not be null or empty!");

        if (!passportNumber.HasValidLength(6, 9))
            throw new InvalidPassportNumberException("Length of Passport number should be greater than 6 and less than or equal to 9 characters!");

        if (!passportNumber.AreAllCharactersLetterOrDigit())
            throw new InvalidPassportNumberException("Passport number should only contain letters and numbers!");
    }


    public static implicit operator PassportNumber(string passportNumber) => Create(passportNumber);
    public static implicit operator string(PassportNumber passportNumber) => passportNumber.ToString();
    public override string ToString() => _passportNumber;
}
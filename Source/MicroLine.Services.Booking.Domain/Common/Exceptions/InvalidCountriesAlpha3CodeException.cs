using MicroLine.Services.Booking.Domain.Common.Enums;

namespace MicroLine.Services.Booking.Domain.Common.Exceptions;

public class InvalidCountriesAlpha3CodeException : DomainException
{
    public override string Code => nameof(InvalidCountriesAlpha3CodeException);

    public InvalidCountriesAlpha3CodeException(string message) : base(message)
    {
    }

    public InvalidCountriesAlpha3CodeException(CountriesAlpha3Code countryCode) : base($"{countryCode} is not a valid country Alpha-3 Code!")
    {
    }
}
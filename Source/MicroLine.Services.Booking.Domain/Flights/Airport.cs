using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights.Exceptions;

namespace MicroLine.Services.Booking.Domain.Flights;

public sealed class Airport : ValueObject
{
    public string IataCode { get; }
    public string Name { get; }
    public BaseUtcOffset BaseUtcOffset { get; }
    public string Country { get; }
    public string Region { get; }
    public string City { get; }

    private Airport(string iataCode, string name, BaseUtcOffset baseUtcOffset, string country, string region, string city)
    {
        IataCode = iataCode;
        Name = name;
        BaseUtcOffset = baseUtcOffset;
        Country = country;
        Region = region;
        City = city;
    }

    public static Airport Create(string iataCode, string name, BaseUtcOffset baseUtcOffset, string country, string region, string city)
    {
        Validate(iataCode, name, country, region, city);

        return new Airport(iataCode.Trim().ToUpperInvariant(),
            name.Trim(),
            baseUtcOffset,
            country.Trim(), region.Trim(), city.Trim());
    }

    private static void Validate(string iataCode, string name, string country, string region, string city)
    {
        ValidateIataCode(iataCode);
        ValidateName(name);
        ValidateLocation(country, region, city);
    }


    private static void ValidateIataCode(string iataCode)
    {
        if (iataCode.IsNullOrWhiteSpace())
            throw new InvalidAirportException("IataCode can not be null or empty!");

        if (!iataCode.Trim().AreAllCharactersEnglishLetter())
            throw new InvalidAirportException("IataCode can only contain letter characters!");

        if (!iataCode.HasValidLength(3))
            throw new InvalidAirportException("IataCode should be 4 characters!");
    }


    private static void ValidateName(string name)
    {
        if (name.IsNullOrWhiteSpace())
            throw new InvalidAirportException("Airport's Name can not be null or empty!");


        if (!name.HasValidLength(4, 60))
            throw new InvalidAirportException("Airport's Name should be greater than 3 and less than 60 characters!");
    }

    private static void ValidateLocation(string country, string region, string city)
    {
        if (country.IsNullOrWhiteSpace() || region.IsNullOrWhiteSpace() || city.IsNullOrWhiteSpace())
            throw new InvalidAirportException("Country, Region and City can not be null or empty in AirportLocation!");
    }

    public override string ToString() => $"{City}-{Name} ({IataCode})";
}

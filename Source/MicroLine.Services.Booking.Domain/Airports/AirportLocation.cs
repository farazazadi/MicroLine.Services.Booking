using MicroLine.Services.Booking.Domain.Airports.Exceptions;
using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Extensions;

namespace MicroLine.Services.Booking.Domain.Airports;

public class AirportLocation : ValueObject
{
    public string Country { get; }
    public string Region { get; }
    public string City { get; }

    private AirportLocation(string country, string region, string city)
    {
        Country = country;
        Region = region;
        City = city;
    }



    public static AirportLocation Create(string country, string region, string city)
    {
        Validate(country, region, city);

        return new AirportLocation(country.Trim(), region.Trim(), city.Trim());
    }

    private static void Validate(string country, string region, string city)
    {
        if (country.IsNullOrWhiteSpace() || region.IsNullOrWhiteSpace() || city.IsNullOrWhiteSpace())
            throw new InvalidAirportLocationException("Country, Region and City can not be null or empty in AirportLocation!");
    }


    public static implicit operator string(AirportLocation address) => address.ToString();

    public override string ToString() => $"{Country}, {Region}, {City}";
}

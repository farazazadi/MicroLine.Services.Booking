using Bogus;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public static class FakeAirport
{
    public static Airport NewFake(
        string? icaoCode = null,
        string? iataCode = null,
        string? name = null,
        BaseUtcOffset? baseUtcOffset = null,
        string? country = null,
        string? region = null,
        string? city = null
    )
    {
        var faker = new Faker();

        iataCode ??= NewFakeIataCode(faker);

        baseUtcOffset ??= FakeBaseUtcOffset.NewFake();

        country ??= NewFakeCountry(faker);
        region ??= NewFakeRegion(faker);
        city ??= NewFakeCity(faker);


        name ??= NewFakeAirportName(faker, city);


        return Airport.Create(iataCode, name, baseUtcOffset, country, region, city);
    }

    public static List<Airport> NewFakeList(int count)
    {
        var airports = new List<Airport>();

        for (var i = 0; i < count; i++)
        {
            var airport = NewFake();
            airports.Add(airport);
        }

        return airports;
    }

    private static string NewFakeIataCode(Faker faker)
    {
        var iataCode = faker.Random.String2(3, 3, RandomSelectionAllowedCharacters.UpperCaseLetters);

        return iataCode;

    }

    private static string NewFakeAirportName(Faker faker, string? city = null)
    {
        city ??= faker.Address.City();
        return $"{city} International Airport";
    }

    private static string NewFakeCountry(Faker faker) => faker.Address.Country();
    private static string NewFakeRegion(Faker faker) => faker.Address.State();
    private static string NewFakeCity(Faker faker) => faker.Address.City();
}
using Bogus;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

namespace MicroLine.Services.Booking.Tests.Common.Fakes;

public static class FakeFlight
{
    public static Flight NewFake(Id? externalId = null, FlightNumber? flightNumber = null,
        Airport? originAirport = null, Airport? destinationAirport = null, Aircraft? aircraft = null,
        DateTime? scheduledUtcDateTimeOfDeparture = null, DateTime? scheduledUtcDateTimeOfArrival = null,
        TimeSpan? estimatedFlightDuration = null, FlightPrice? basePrices = null, Flight.FlightStatus? status = null)
    {
        var faker = new Faker();

        externalId ??= Id.Create();

        flightNumber ??= NewFakeFlightNumber(faker);

        originAirport ??= FakeAirport.NewFake();
        destinationAirport ??= FakeAirport.NewFake();

        aircraft ??= FakeAircraft.NewFake();

        scheduledUtcDateTimeOfDeparture ??= DateTime.UtcNow.AddDays(4);
        int flightLength = faker.Random.Int(2,10);
        scheduledUtcDateTimeOfArrival ??= scheduledUtcDateTimeOfDeparture.Value.AddHours(flightLength);

        estimatedFlightDuration ??= scheduledUtcDateTimeOfArrival.Value - scheduledUtcDateTimeOfDeparture.Value;

        basePrices ??= NewFakeFlightPrice(faker);


        var flight = Flight.Create(
            externalId,
            flightNumber,
            originAirport,
            destinationAirport,
            aircraft,
            scheduledUtcDateTimeOfDeparture.Value,
            scheduledUtcDateTimeOfArrival.Value,
            estimatedFlightDuration.Value,
            basePrices,
            Flight.FlightStatus.Scheduled
        );

        return flight;
    }


    public static List<Flight> ScheduleNewFakeFlights(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => NewFake())
            .ToList();
    }


    private static FlightNumber NewFakeFlightNumber(Faker faker)
    {
        var flightNumber = faker.Random.String2(3, RandomSelectionAllowedCharacters.UpperCaseLetters)
            + faker.Random.UInt(101,999);

        return flightNumber;
    }

    private static FlightPrice NewFakeFlightPrice(Faker faker)
    {
        var flightPrice = FlightPrice.Create(
            Money.Of(faker.Random.Int(100,199), Money.CurrencyType.UnitedStatesDollar),
            Money.Of(faker.Random.Int(200, 299), Money.CurrencyType.UnitedStatesDollar),
            Money.Of(faker.Random.Int(300, 399), Money.CurrencyType.UnitedStatesDollar)
        );

        return flightPrice;
    }
}
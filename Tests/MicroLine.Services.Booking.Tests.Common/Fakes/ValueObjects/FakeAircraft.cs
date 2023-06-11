using Bogus;
using MicroLine.Services.Booking.Domain.Flights;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public static class FakeAircraft
{
    public static Aircraft NewFake(
        int? economyClassCapacity = null,
        int? businessClassCapacity = null,
        int? firstClassCapacity = null
        )
    {
        var faker = new Faker();

        var model = NewFakeAircraftModel(faker);

        var aircraft = Aircraft.Create(model
            , economyClassCapacity ?? NewFakeEconomyClassCapacity()
            , businessClassCapacity ?? NewFakeBusinessClassCapacity()
            , firstClassCapacity ?? NewFakeFirstClassCapacity()
        );

        return aircraft;
    }

    public static List<Aircraft> NewFakeList(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => NewFake())
            .ToList();
    }


    private static string NewFakeAircraftModel(Faker faker)
    {
        var manufacturers = new[] { "Airbus", "Boeing", "Lockheed Martin", "Bombardier", "Embraer", "Tupoloev" };

        var airbusModels = new[] { "A350", "A380", "A330", "A321", "A320", "A300" };
        var boeingModels = new[] { "787", "777", "757", "747", "737 MAX", "707" };
        var lockheedMartinModels = new[] { "LM-100J" };
        var bombardierModels = new[] { "CHALLENGER 300", "CHALLENGER 605", "GLOBAL 5000", "GLOBAL 6000" };
        var embraerModels = new[] { "E175-E2", "E195", "ERJ135" };
        var tupoloevModels = new[] { "Tu-154", "Tu-334" };

        string manufacturer = faker.PickRandom(manufacturers);

        var model = manufacturer switch
        {
            "Airbus" => faker.PickRandom(airbusModels),
            "Boeing" => faker.PickRandom(boeingModels),
            "Lockheed Martin" => faker.PickRandom(lockheedMartinModels),
            "Bombardier" => faker.PickRandom(bombardierModels),
            "Embraer" => faker.PickRandom(embraerModels),
            "Tupoloev" => faker.PickRandom(tupoloevModels),
            _ => throw new NotImplementedException()
        };

        return $"{manufacturer} {model}";
    }

    private static int NewFakeEconomyClassCapacity() => Random.Shared.Next(100, 150);
    private static int NewFakeBusinessClassCapacity() => Random.Shared.Next(30, 50);
    private static int NewFakeFirstClassCapacity() => Random.Shared.Next(10, 20);

}
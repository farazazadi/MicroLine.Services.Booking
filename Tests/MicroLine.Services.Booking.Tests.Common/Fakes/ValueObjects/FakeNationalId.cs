using Bogus;
using MicroLine.Services.Booking.Domain.Passengers;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public static class FakeNationalId
{
    public static NationalId NewFake(Faker? faker = null)
    {
        faker ??= new Faker();

        var id = faker.Random.String2(8, 20, RandomSelectionAllowedCharacters.DigitsAndUpperCaseLetters);

        return NationalId.Create(id);
    }
}
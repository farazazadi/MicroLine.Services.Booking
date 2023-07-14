using Bogus;
using MicroLine.Services.Booking.Domain.Passengers;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public static class FakePassportNumber
{
    public static PassportNumber NewFake(Faker? faker = null)
    {
        faker ??= new Faker();

        var number = faker.Random.String2(6, 9, RandomSelectionAllowedCharacters.DigitsAndUpperCaseLetters);

        return PassportNumber.Create(number);
    }
}
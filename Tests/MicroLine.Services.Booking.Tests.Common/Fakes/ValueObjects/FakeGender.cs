using Bogus;
using Bogus.DataSets;
using MicroLine.Services.Booking.Domain.Common.Enums;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public static class FakeGender
{
    public static Gender PickRandom(Faker? faker = null)
    {
        faker ??= new Faker();

        return faker.PickRandom<Gender>();
    }

    internal static Name.Gender ToBogusGender(this Gender gender,
        Name.Gender defaultMember = Name.Gender.Female)
    {
        return gender switch
        {
            Gender.Female => Name.Gender.Female,
            Gender.Male => Name.Gender.Male,
            Gender.Other => Name.Gender.Female,
            _ => defaultMember,
        };
    }
}
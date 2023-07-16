using Bogus;
using MicroLine.Services.Booking.Domain.Common.Enums;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Passengers;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public class FakePassport
{
    public static Passport NewFake(FullName? fullName = null, Gender? gender = null,
        CountriesAlpha3Code? countryCode = null, PassportNumber? passportNumber = null,
        Date? birthDate = null, Date? issueDate = null, Date? expiryDate = null,
        Faker? faker = null)
    {
        faker ??= new Faker();

        DateTime today = DateTime.Today;


        gender ??= FakeGender.PickRandom();
        fullName ??= FakeFullName.NewFake(gender.Value, faker);
        countryCode ??= faker.PickRandom<CountriesAlpha3Code>();
        passportNumber ??= FakePassportNumber.NewFake(faker);
        birthDate ??= FakeDate.NewFake(faker);
        issueDate ??= today.AddYears(-3);
        expiryDate ??= today.AddYears(2);

        var passport = Passport.Create(
            fullName,
            gender.Value,
            countryCode.Value,
            passportNumber,
            birthDate,
            issueDate,
            expiryDate
        );

        return passport;
    }
}
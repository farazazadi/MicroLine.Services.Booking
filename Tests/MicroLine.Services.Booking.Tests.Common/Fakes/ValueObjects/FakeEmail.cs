using Bogus;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Passengers;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public static class FakeEmail
{
    public static Email NewFake(string firstName, string lastName, Faker? faker = null)
    {
        faker ??= new Faker();

        var email = faker.Internet.Email(firstName, lastName);

        return Email.Create(email);
    }

    public static Email NewFake(FullName firstName, Faker? faker = null)
        => NewFake(firstName.FirstName, firstName.FirstName, faker);
}
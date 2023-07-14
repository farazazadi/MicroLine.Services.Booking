using Bogus;
using MicroLine.Services.Booking.Domain.Passengers;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public static class FakeContactNumber
{
    public static ContactNumber NewFake(Faker? faker = null)
    {
        faker ??= new Faker();

        var number = faker.Phone.PhoneNumber("+1###########");

        return ContactNumber.Create(number);
    }
}
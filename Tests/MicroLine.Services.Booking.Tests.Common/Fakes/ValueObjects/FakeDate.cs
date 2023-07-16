
using Bogus;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public static class FakeDate
{
    public static Date NewFake(Faker? faker = null)
    {
        faker ??= new Faker();

        var startDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-55));
        var endDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));

        return faker.Date.BetweenDateOnly(startDate, endDate);
    }
}
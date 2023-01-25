using Bogus;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

public static class FakeBaseUtcOffset
{
    public static BaseUtcOffset NewFake()
    {
        var faker = new Faker();

        var systemTimeZones = TimeZoneInfo.GetSystemTimeZones();
        var timeZoneInfo = faker.PickRandom(systemTimeZones, 1)
            .First();

        var hours = timeZoneInfo.BaseUtcOffset.Hours;
        var minutes = timeZoneInfo.BaseUtcOffset.Minutes;

        return BaseUtcOffset.Create(hours, minutes);
    }
}
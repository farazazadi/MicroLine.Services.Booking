using Bogus;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

namespace MicroLine.Services.Booking.Tests.Common.Fakes;

public static class FakePassenger
{
    public static Passenger NewFake(Id? relatedUserExternalId = null, NationalId? nationalId = null,
        Passport? passport = null, Email? email = null, ContactNumber? contactNumber = null)
    {
        var faker = new Faker();

        relatedUserExternalId ??= Id.Create();

        nationalId ??= FakeNationalId.NewFake(faker);

        passport ??= FakePassport.NewFake(faker: faker);

        email ??= FakeEmail.NewFake(passport.FullName, faker);

        contactNumber ??= FakeContactNumber.NewFake(faker);

        return Passenger.Create(relatedUserExternalId, nationalId, passport, email, contactNumber);
    }

    public static List<Passenger> NewFakeList(int count, Id? relatedUserExternalId = null)
        => Enumerable
            .Range(1, count)
            .Select(_ => NewFake(relatedUserExternalId: relatedUserExternalId))
            .ToList();
}
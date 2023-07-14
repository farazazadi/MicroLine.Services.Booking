using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Exceptions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Domain.Passengers;

public sealed class Passenger : AggregateRoot
{
    public Id RelatedUserExternalId { get; private set; }
    public NationalId NationalId { get; private set; }
    public Passport Passport { get; private set; }
    public Email Email { get; private set; }
    public ContactNumber ContactNumber { get; private set; }

    private Passenger(Id relatedUserExternalId, NationalId nationalId, Passport passport,
        Email email, ContactNumber contactNumber)
    {
        RelatedUserExternalId = relatedUserExternalId;
        NationalId = nationalId;
        Passport = passport;
        Email = email;
        ContactNumber = contactNumber;
    }

    public static Passenger Create(Id relatedUserExternalId, NationalId nationalId, Passport passport,
        Email email, ContactNumber contactNumber)
    {
        ValidateRelatedUserExternalId(relatedUserExternalId);

        return new Passenger(relatedUserExternalId, nationalId, passport, email, contactNumber);
    }

    private static void ValidateRelatedUserExternalId(Id relatedUserExternalId)
    {
        if (relatedUserExternalId == Id.Transient)
            throw new InvalidIdException($"{nameof(relatedUserExternalId)} is transient!");
    }
}
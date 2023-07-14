﻿using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Passengers.Exceptions;

namespace MicroLine.Services.Booking.Domain.Passengers;

public sealed class FullName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static FullName Create(string firstName, string lastName)
    {
        Validate(firstName, lastName);

        return new FullName(firstName.Trim(), lastName.Trim());
    }

    private static void Validate(string firstName, string lastName)
    {
        if (firstName.IsNullOrWhiteSpace() || lastName.IsNullOrWhiteSpace())
            throw new InvalidFullNameException("First and last name can not be null or empty!");

        if (!firstName.AreAllCharactersLetter() || !lastName.AreAllCharactersLetter())
            throw new InvalidFullNameException("First and last name can only contain letter characters!");

        if (!firstName.HasValidLength(3, 50))
            throw new InvalidFullNameException("Length of FirstName should be greater than 3 and less than or equal to 50 characters!");

        if (!lastName.HasValidLength(3, 50))
            throw new InvalidFullNameException("Length of LastName should be greater than 3 and less than or equal to 50 characters!");
    }

    public override string ToString() => $"{FirstName} {LastName}";

}
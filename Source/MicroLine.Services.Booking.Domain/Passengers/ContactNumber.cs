using System.Text.RegularExpressions;
using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Passengers.Exceptions;

namespace MicroLine.Services.Booking.Domain.Passengers;

public sealed class ContactNumber : ValueObject
{
    private readonly string _contactNumber;

    private ContactNumber(string contactNumber)
    {
        _contactNumber = contactNumber;
    }

    public static ContactNumber Create(string contactNumber)
    {
        Validate(contactNumber);

        return new ContactNumber(contactNumber.Trim());
    }

    private static void Validate(string contactNumber)
    {
        if (contactNumber.IsNullOrWhiteSpace())
            throw new InvalidContactNumberException("Contact number can not be null or empty!");

        var pattern = @"^(?:00|\+)(?!0)\d{10,15}$";

        if (!Regex.IsMatch(contactNumber, pattern, RegexOptions.Compiled))
            throw new InvalidContactNumberException($"Contact number is not in the correct format!");
    }


    public static implicit operator ContactNumber(string contactNumber) => Create(contactNumber);
    public static implicit operator string(ContactNumber contactNumber) => contactNumber.ToString();

    public override string ToString() => _contactNumber;
}
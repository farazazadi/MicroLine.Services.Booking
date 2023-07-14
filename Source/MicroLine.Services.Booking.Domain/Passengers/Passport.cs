using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Enums;
using MicroLine.Services.Booking.Domain.Common.Exceptions;
using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Passengers.Exceptions;

namespace MicroLine.Services.Booking.Domain.Passengers;

public class Passport : ValueObject
{

    public FullName FullName { get; }
    public Gender Gender { get; }
    public CountriesAlpha3Code CountryCode { get; }
    public PassportNumber PassportNumber { get; }
    public Date BirthDate { get; }
    public Date IssueDate { get; }
    public Date ExpiryDate { get; }


    private Passport(FullName fullName, Gender gender, CountriesAlpha3Code countryCode,
        PassportNumber passportNumber, Date birthDate, Date issueDate, Date expiryDate)
    {
        FullName = fullName;
        Gender = gender;
        CountryCode = countryCode;
        PassportNumber = passportNumber;
        BirthDate = birthDate;
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
    }
    public static Passport Create(FullName fullName, Gender gender, CountriesAlpha3Code countryCode,
        PassportNumber passportNumber, Date birthDate, Date issueDate, Date expiryDate)
    {

        Validate(gender, countryCode, birthDate, issueDate, expiryDate);

        return new Passport(fullName, gender, countryCode, passportNumber, birthDate, issueDate, expiryDate);
    }

    private static void Validate(Gender gender, CountriesAlpha3Code countryCode, Date birthDate, Date issueDate, Date expiryDate)
    {
        if(!gender.IsValidEnumMember())
            throw new InvalidGenderException(gender);

        if(!countryCode.IsValidEnumMember())
            throw new InvalidCountriesAlpha3CodeException(countryCode);


        ValidateBirthDate(birthDate);
        ValidateIssueDate(issueDate, expiryDate);
        ValidateExpiryDate(issueDate, expiryDate);
    }

    private static void ValidateBirthDate(Date birthDate)
    {
        Date today = DateTime.Today;

        if (birthDate >= today)
            throw new InvalidPassportBirthDateException(birthDate);
    }

    private static void ValidateIssueDate(Date issueDate, Date expiryDate)
    {
        Date today = DateTime.Today;
        Date tenYearsAgo = DateTime.Today.AddYears(-10);


        if (issueDate > today)
            throw new InvalidPassportIssueDateException(
                $"Passport's IssueDate ({issueDate}) must be smaller than Today ({today})!");


        if (issueDate < tenYearsAgo)
            throw new InvalidPassportIssueDateException(
                $"Passport's IssueDate ({issueDate}) can not be older than 10 years ago ({tenYearsAgo})!");


        if (issueDate >= expiryDate)
            throw new InvalidPassportIssueDateException(
                $"Passport's IssueDate ({issueDate}) must be smaller than its ExpiryDate ({expiryDate})!");
    }


    private static void ValidateExpiryDate(Date issueDate, Date expiryDate)
    {
        Date validityDate = ((DateOnly)issueDate).AddYears(10);

        if (expiryDate > validityDate)
            throw new InvalidPassportExpiryDateException(
                $"Passport's ExpiryDate ({expiryDate}) cannot exceed 10 years after the date of issuance ({issueDate})!");
    }
}
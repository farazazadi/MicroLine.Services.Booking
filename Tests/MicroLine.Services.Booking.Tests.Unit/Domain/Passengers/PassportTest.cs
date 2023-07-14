using MicroLine.Services.Booking.Domain.Common.Enums;
using MicroLine.Services.Booking.Domain.Common.Exceptions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Domain.Passengers.Exceptions;
using MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;

namespace MicroLine.Services.Booking.Tests.Unit.Domain.Passengers;

public class PassportTest
{
    [Theory]
    [InlineData((Gender)5)]
    [InlineData((Gender)3)]
    [InlineData((Gender)(-1))]
    public void Passport_ShouldThrowInvalidGenderException_WhenGenderIsNotValid(Gender gender)
    {
        // Given
        // When
        Func<Passport> func = () => FakePassport.NewFake(gender: gender);

        // Then
        func.Should().ThrowExactly<InvalidGenderException>()
            .And.Code.Should().Be(nameof(InvalidGenderException));
    }


    [Theory]
    [InlineData((CountriesAlpha3Code)500)]
    [InlineData((CountriesAlpha3Code)300)]
    [InlineData((CountriesAlpha3Code)(-1))]
    public void Passport_ShouldThrowInvalidCountriesAlpha3CodeException_WhenCountryCodeIsNotValid(CountriesAlpha3Code countryCode)
    {
        // Given
        // When
        Func<Passport> func = () => FakePassport.NewFake(countryCode: countryCode);

        // Then
        func.Should().ThrowExactly<InvalidCountriesAlpha3CodeException>()
            .And.Code.Should().Be(nameof(InvalidCountriesAlpha3CodeException));
    }


    public static TheoryData<Date> InvalidBirthDates = new()
    {
        DateTime.Today,
        DateTime.Today.AddDays(1),
        DateTime.Today.AddYears(5)
    };

    [Theory, MemberData(nameof(InvalidBirthDates))]
    public void Passport_ShouldThrowInvalidPassportBirthDateException_WhenBirthDateIsGreaterOrEqualToToday(Date birthDate)
    {
        // Given
        // When
        Func<Passport> func = ()=> FakePassport.NewFake(birthDate: birthDate);

        // Then
        func.Should().ThrowExactly<InvalidPassportBirthDateException>()
            .And.Code.Should().Be(nameof(InvalidPassportBirthDateException));
    }


    [Fact]
    public void Passport_ShouldThrowInvalidPassportIssueDateException_WhenIssueDateIsGreaterThanToday()
    {
        // Given
        Date dateGreaterThanToday = DateTime.Today
            .AddDays(Random.Shared.Next(1, 2000));

        // When
        Func<Passport> func = () => FakePassport.NewFake(issueDate: dateGreaterThanToday);

        // Then
        func.Should().ThrowExactly<InvalidPassportIssueDateException>()
            .And.Code.Should().Be(nameof(InvalidPassportIssueDateException));
    }


    [Fact]
    public void Passport_ShouldThrowInvalidPassportIssueDateException_WhenIssueDateIsOlderThanTenYearsAgo()
    {
        // Given
        Date dateOlderThanLastTenYears = DateTime.Today
            .AddYears(-10)
            .AddDays(Random.Shared.Next(-2000, -1));

        // When
        Func<Passport> func = () => FakePassport.NewFake(issueDate: dateOlderThanLastTenYears);

        // Then
        func.Should().ThrowExactly<InvalidPassportIssueDateException>()
            .And.Code.Should().Be(nameof(InvalidPassportIssueDateException));
    }


    public static TheoryData<Date, Date> InvalidIssueAndExpiryDates = new()
    {
        // IssueDate            ExpiryDate
        {DateTime.Today, DateTime.Today},             // IssueDate can not be equal to ExpiryDate!
        {DateTime.Today.AddDays(1), DateTime.Today}, // IssueDate can not be Greater than ExpiryDate!
    };

    [Theory, MemberData(nameof(InvalidIssueAndExpiryDates))]
    public void Passport_ShouldThrowInvalidPassportIssueDateException_WhenIssueDateIsGreaterOrEqualToExpiryDate(Date issueDate, Date expiryDate)
    {
        // Given
        // When
        Func<Passport> func = () => FakePassport.NewFake(issueDate: issueDate, expiryDate: expiryDate);

        // Then
        func.Should().ThrowExactly<InvalidPassportIssueDateException>()
            .And.Code.Should().Be(nameof(InvalidPassportIssueDateException));
    }



    [Fact]
    public void Passport_ShouldThrowInvalidPassportExpiryDateException_WhenExpiryDateExceedsA10YearValidityDate()
    {
        // Given
        Date dateOutOfValidityDate = DateTime.Today
            .AddYears(10)
            .AddDays(Random.Shared.Next(1, 2000));

        // When
        Func<Passport> func = () => FakePassport.NewFake(expiryDate: dateOutOfValidityDate);


        // Then
        func.Should().ThrowExactly<InvalidPassportExpiryDateException>()
            .And.Code.Should().Be(nameof(InvalidPassportExpiryDateException));
    }

}
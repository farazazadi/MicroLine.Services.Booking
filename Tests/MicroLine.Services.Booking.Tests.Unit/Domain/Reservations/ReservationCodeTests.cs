using MicroLine.Services.Booking.Domain.Reservations;
using MicroLine.Services.Booking.Domain.Reservations.Exceptions;

namespace MicroLine.Services.Booking.Tests.Unit.Domain.Reservations;

public class ReservationCodeTests
{


    public static TheoryData<string?> NullOrEmptyStrings = new()
    {
        " ",
        "        ",
        string.Empty,
        null
    };


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void ReservationCode_ShouldThrowInvalidReservationCodeException_WhenInputIsNullOrEmpty(string reservationCode)
    {
        // Given
        // When
        Func<ReservationCode> func = () => ReservationCode.Create(reservationCode);

        // Then
        func.Should().ThrowExactly<InvalidReservationCodeException>()
            .And.Code.Should().Be(nameof(InvalidReservationCodeException));
    }


    public static TheoryData<string> ReservationCodesWithInvalidLength = new()
    {
        "1",
        " a",
        " B ",
        "abcdefg",
        " abcdefg  ",
        "abcdefgh1",
        "A2355555555555555555555555555"
    };


    [Theory, MemberData(nameof(ReservationCodesWithInvalidLength))]
    public void ReservationCode_ShouldThrowInvalidReservationCodeException_WhenLengthOfInputIsNot8(string reservationCode)
    {
        // Given
        // When
        Func<ReservationCode> func = () => ReservationCode.Create(reservationCode);

        // Then
        func.Should().ThrowExactly<InvalidReservationCodeException>()
            .And.Code.Should().Be(nameof(InvalidReservationCodeException));
    }



    public static TheoryData<string> ReservationCodesWithInvalidCharacters = new()
    {
        "^A23BCDF",
        "A23BCDF(",
        "125!$325",
        "JM6%jhbf"
    };


    [Theory, MemberData(nameof(ReservationCodesWithInvalidCharacters))]
    public void ReservationCode_ShouldThrowInvalidReservationCodeException_WhenInputContainsNonAlphaNumericCharacter(string reservationCode)
    {
        // Given
        // When
        Func<ReservationCode> func = () => ReservationCode.Create(reservationCode);

        // Then
        func.Should().ThrowExactly<InvalidReservationCodeException>()
            .And.Code.Should().Be(nameof(InvalidReservationCodeException));
    }



    [Fact]
    public void ReservationCode_ShouldBeCreatedAsExpected()
    {
        // Given
        // When
        ReservationCode reservationCode = ReservationCode.Create();

        // Then
        var reservationCodeString = reservationCode.ToString();

        reservationCodeString.Length.Should().Be(8);
        reservationCodeString.Should().NotBeLowerCased();
    }


}
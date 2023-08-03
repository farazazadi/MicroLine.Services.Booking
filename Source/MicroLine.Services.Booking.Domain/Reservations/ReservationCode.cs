using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Reservations.Exceptions;

namespace MicroLine.Services.Booking.Domain.Reservations;

public class ReservationCode : ValueObject
{
    private readonly string _reservationCode;


    private ReservationCode(string reservationCode)
    {
        _reservationCode = reservationCode;
    }


    public static ReservationCode Create()
    {
        // todo: Uniqueness of this code is not 100% guaranteed! it should be replaced.
        var reservationCode = Guid.NewGuid().GetHashCode().ToString("X");

        return new ReservationCode(reservationCode);
    }


    public static ReservationCode Create(string reservationCode)
    {
        if (reservationCode.IsNullOrWhiteSpace())
            throw new InvalidReservationCodeException("The ReservationCode can not be null or empty!");

        if(!reservationCode.HasValidLength(8))
            throw new InvalidReservationCodeException("The length of ReservationCode should be 8!");

        if(!reservationCode.AreAllCharactersLetterOrDigit())
            throw new InvalidReservationCodeException("The ReservationCode should only consist of letters or digits!");

        return new ReservationCode(reservationCode);
    }


    public static implicit operator string(ReservationCode reservationCode) => reservationCode._reservationCode;

    public static implicit operator ReservationCode(string reservationCode) => Create(reservationCode);

    public override string ToString() => _reservationCode;
}
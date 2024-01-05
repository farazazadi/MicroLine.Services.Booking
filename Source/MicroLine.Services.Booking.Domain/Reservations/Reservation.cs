using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Domain.Reservations.Events;
using MicroLine.Services.Booking.Domain.Reservations.Exceptions;

namespace MicroLine.Services.Booking.Domain.Reservations;


public sealed class Reservation : AggregateRoot
{
    public enum ReservationStatus : byte
    {
        Held = 0,
        Confirmed = 1,
        Canceled = 2
    }


    public ReservationCode ReservationCode { get; private set; }

    public Flight Flight { get; private set; }

    private List<Passenger> _passengers;
    public IReadOnlyList<Passenger> Passengers
    {
        get => _passengers;
        private set => _passengers = value.ToList();
    }

    public ReservationStatus Status { get; private set; }


    private Reservation(ReservationCode reservationCode, Flight flight, List<Passenger> passengers, ReservationStatus reservationStatus)
    {
        ReservationCode = reservationCode;
        Flight = flight;
        _passengers = passengers;
        Status = reservationStatus;
    }


    public static Reservation Hold(Flight flight, List<Passenger> passengers)
    {
        if(passengers.Count == 0)
            throw new InvalidReservationException("The reservation can not be held without having at least one passenger!");


        var reservation = new Reservation(
            ReservationCode.Create(),
            flight,
            passengers,
            ReservationStatus.Held
            );

        reservation.AddEvent(new ReservationHeldEvent(reservation));

        return reservation;
    }
}
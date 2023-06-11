using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights.Exceptions;

namespace MicroLine.Services.Booking.Domain.Flights;

public class Flight : AggregateRoot
{
    public enum FlightStatus
    {
        Unknown = 0,
        Scheduled = 1,
        Canceled = 2,
        Departed = 3,
        Landed = 4,
        EmergencyLanding = 5
    }

    public Id ExternalId { get; private set; }
    public FlightNumber FlightNumber { get; private set; }
    public Airport OriginAirport { get; private set; }
    public Airport DestinationAirport { get; private set; }
    public Aircraft Aircraft { get; private set; }
    public DateTime ScheduledUtcDateTimeOfDeparture { get; private set; }
    public DateTime ScheduledUtcDateTimeOfArrival { get; private set; }
    public TimeSpan EstimatedFlightDuration { get; private set; }
    public FlightPrice Prices { get; private set; }

    public FlightStatus Status { get; private set; }

    private Flight(Id externalId, FlightNumber flightNumber,
        Airport originAirport, Airport destinationAirport, Aircraft aircraft,
        DateTime scheduledUtcDateTimeOfDeparture, DateTime scheduledUtcDateTimeOfArrival, TimeSpan estimatedFlightDuration,
        FlightPrice prices, FlightStatus status)
    {
        ExternalId = externalId;
        FlightNumber = flightNumber;
        OriginAirport = originAirport;
        DestinationAirport = destinationAirport;
        Aircraft = aircraft;
        ScheduledUtcDateTimeOfDeparture = scheduledUtcDateTimeOfDeparture.RemoveSecondsAndSmallerTimeUnites();
        EstimatedFlightDuration = estimatedFlightDuration;
        ScheduledUtcDateTimeOfArrival = scheduledUtcDateTimeOfArrival;
        Prices = prices;
        Status = status;
    }


    private static void CheckScheduledUtcDateTimes(DateTime scheduledUtcDateTimeOfDeparture, DateTime scheduledUtcDateTimeOfArrival)
    {
        var now = DateTime.UtcNow.RemoveSecondsAndSmallerTimeUnites();

        if (scheduledUtcDateTimeOfDeparture <= now)
            throw new InvalidScheduledDateTimeOfDeparture($"The scheduled DateTime (UTC) of departure ({scheduledUtcDateTimeOfDeparture:f}) cannot be in the past!");

        if(scheduledUtcDateTimeOfDeparture >= scheduledUtcDateTimeOfArrival)
            throw new InvalidScheduledDateTimeOfDeparture($"The scheduled DateTime (UTC) of departure ({scheduledUtcDateTimeOfDeparture:f}) cannot be greater or equal to The scheduled DateTime (UTC) of arrival ({scheduledUtcDateTimeOfArrival:f})!");

    }



    public static Flight Create(Id externalId, FlightNumber flightNumber,
        Airport originAirport, Airport destinationAirport, Aircraft aircraft,
        DateTime scheduledUtcDateTimeOfDeparture, DateTime scheduledUtcDateTimeOfArrival, TimeSpan estimatedFlightDuration,
        FlightPrice prices, FlightStatus status)
    {

        CheckScheduledUtcDateTimes(scheduledUtcDateTimeOfDeparture, scheduledUtcDateTimeOfArrival);

        var flight = new Flight(externalId, flightNumber, originAirport, destinationAirport, aircraft,
            scheduledUtcDateTimeOfDeparture, scheduledUtcDateTimeOfArrival, estimatedFlightDuration, prices, status);

        return flight;
    }
}
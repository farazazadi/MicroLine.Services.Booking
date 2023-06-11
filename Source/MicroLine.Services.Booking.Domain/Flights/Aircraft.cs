using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Flights.Exceptions;

namespace MicroLine.Services.Booking.Domain.Flights;

public sealed class Aircraft : ValueObject
{
    public string Model { get; }

    public int EconomyClassCapacity { get; }

    public int BusinessClassCapacity { get; }

    public int FirstClassCapacity { get; }


    private Aircraft(string model, int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        Model = model;
        EconomyClassCapacity = economyClassCapacity;
        BusinessClassCapacity = businessClassCapacity;
        FirstClassCapacity = firstClassCapacity;
    }


    public static Aircraft Create (string model, int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        Validate(model, economyClassCapacity, businessClassCapacity, firstClassCapacity);

        return new Aircraft(model, economyClassCapacity, businessClassCapacity, firstClassCapacity);
    }

    private static void Validate(string model, int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        ValidateModel(model);

        ValidateCapacity(economyClassCapacity, businessClassCapacity, firstClassCapacity);
    }

    private static void ValidateModel(string model)
    {
        if (model.IsNullOrWhiteSpace())
            throw new InvalidAircraftException("Aircraft model could not be null or empty!");

        if (!model.HasValidLength(5, 25))
            throw new InvalidAircraftException(
                "Aircraft model's length could not be greater than 25 or less than 5 characters!");
    }

    private static void ValidateCapacity(int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        if (economyClassCapacity < 0)
            throw new InvalidAircraftException("Count of economy class seats can not be negative!");
        if (businessClassCapacity < 0)
            throw new InvalidAircraftException("Count of business class seats can not be negative!");
        if (firstClassCapacity < 0)
            throw new InvalidAircraftException("Count of business class seats can not be negative!");

        if (economyClassCapacity + businessClassCapacity + firstClassCapacity < 1)
            throw new InvalidAircraftException("passenger seating capacity should be greater than 0!");
    }
    public override string ToString() => Model;
}
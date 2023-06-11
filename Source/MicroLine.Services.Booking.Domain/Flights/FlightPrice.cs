using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Flights.Exceptions;

namespace MicroLine.Services.Booking.Domain.Flights;

public class FlightPrice : ValueObject
{
    public Money EconomyClass { get; }

    public Money BusinessClass { get; }

    public Money FirstClass { get; }

    private FlightPrice(Money economyClass, Money businessClass, Money firstClass)
    {
        EconomyClass = economyClass;
        BusinessClass = businessClass;
        FirstClass = firstClass;
    }


    public static FlightPrice Create(Money economyClass, Money businessClass, Money firstClass)
    {
        Validate(economyClass, businessClass, firstClass);

        return new FlightPrice(economyClass, businessClass, firstClass);
    }

    private static void Validate(Money economyClass, Money businessClass, Money firstClass)
    {
        var moneys = new[]
        {
            economyClass.Currency, businessClass.Currency, firstClass.Currency
        };

        var areAllCurrenciesSame = moneys.GroupBy(m => m).Count() == 1;

        if (!areAllCurrenciesSame)
            throw new InvalidFlightPriceException("All prices should use same CurrencyType!");
    }


    public override string ToString() =>
        $"EconomyClass: {EconomyClass}, BusinessClass: {BusinessClass}, FirstClass: {FirstClass}";
}
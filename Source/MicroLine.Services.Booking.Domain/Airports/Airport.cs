using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Domain.Airports;

public class Airport : AggregateRoot
{
    public IcaoCode IcaoCode { get; private set; }
    public IataCode IataCode { get; private set; }
    public AirportName Name { get; private set; }
    public BaseUtcOffset BaseUtcOffset { get; private set; }
    public AirportLocation Location { get; private set; }

    private Airport(IcaoCode icaoCode, IataCode iataCode, AirportName name, BaseUtcOffset baseUtcOffset, AirportLocation Location)
    {
        Id = $"{iataCode}-{Location.City}";

        IcaoCode = icaoCode;
        IataCode = iataCode;
        Name = name;
        BaseUtcOffset = baseUtcOffset;
        this.Location = Location;
    }

    public static Airport Create(IcaoCode icaoCode, IataCode iataCode, AirportName name,
        BaseUtcOffset baseUtcOffset, AirportLocation airportLocation)
    {
        return new Airport(icaoCode, iataCode, name, baseUtcOffset, airportLocation);
    }


}

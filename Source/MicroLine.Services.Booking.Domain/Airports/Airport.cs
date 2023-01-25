using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Domain.Airports;

public class Airport : AggregateRoot
{
    public Id ExternalId { get; private set; }
    public IcaoCode IcaoCode { get; private set; }
    public IataCode IataCode { get; private set; }
    public AirportName Name { get; private set; }
    public BaseUtcOffset BaseUtcOffset { get; private set; }
    public AirportLocation Location { get; private set; }

    private Airport(Id externalId, IcaoCode icaoCode, IataCode iataCode, AirportName name, BaseUtcOffset baseUtcOffset, AirportLocation location)
    {
        Id = $"{iataCode}-{location.City}";
        ExternalId = externalId;

        IcaoCode = icaoCode;
        IataCode = iataCode;
        Name = name;
        BaseUtcOffset = baseUtcOffset;
        Location = location;
    }

    public static Airport Create(Id externalId, IcaoCode icaoCode, IataCode iataCode, AirportName name,
        BaseUtcOffset baseUtcOffset, AirportLocation airportLocation)
    {
        return new Airport(externalId, icaoCode, iataCode, name, baseUtcOffset, airportLocation);
    }


}

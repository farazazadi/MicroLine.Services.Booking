using MicroLine.Services.Booking.Domain.Airports;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Configurations;

namespace MicroLine.Services.Booking.WebApi.Features.Airports;

internal class AirportMongoConfigurations : IMongoConfiguration
{
    public void Configure()
    {

        ValueConvertor<IcaoCode, string>
            .Register(icaoCode => icaoCode, icaoCodeString => icaoCodeString);

        ValueConvertor<IataCode, string>
            .Register(iataCode => iataCode, iataCodeString => iataCodeString);

        ValueConvertor<AirportName, string>
            .Register(airportName => airportName, airportNameString => airportNameString);

    }
}

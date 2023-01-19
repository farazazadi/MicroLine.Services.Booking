using Mapster;
using MicroLine.Services.Booking.Domain.Airports;

namespace MicroLine.Services.Booking.WebApi.Features.Airports;

internal class AirportMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Airport, CreateAirport.Command>()
            .Map(command => command.IcaoCode, airport => airport.IcaoCode)
            .Map(command => command.IataCode, airport => airport.IataCode)
            .Map(command => command.Name, airport => airport.Name)
            .Map(command => command.BaseUtcOffset, airport => airport.BaseUtcOffset)
            .Map(command => command.Location, airport => airport.Location);

    }

}
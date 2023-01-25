using Mapster;
using MicroLine.Services.Booking.Domain.Airports;
using MicroLine.Services.Booking.WebApi.Infrastructure.Integration.IncomingEvents.Airline;

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


        config.NewConfig<AirportCreatedIntegrationEvent, CreateAirport.Command>()
            .Map(command => command.ExternalId, integrationEvent => integrationEvent.Id)
            .Map(command => command.IcaoCode, integrationEvent => integrationEvent.IcaoCode)
            .Map(command => command.IataCode, integrationEvent => integrationEvent.IataCode)
            .Map(command => command.Name, integrationEvent => integrationEvent.Name)
            .Map(command => command.BaseUtcOffset, integrationEvent => integrationEvent.BaseUtcOffset)
            .Map(command => command.Location, integrationEvent => integrationEvent.Location)
            ;
    }

}
using Mapster;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.WebApi.Features.Passengers.DataTransferObjects;

namespace MicroLine.Services.Booking.WebApi.Features.Passengers;

internal sealed class PassengerMappingsConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Passenger, PassengerDto>()
            .Map(dto => dto.AuditingDetails, passenger => passenger);
    }
}
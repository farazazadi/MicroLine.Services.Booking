using MicroLine.Services.Booking.Domain.Reservations;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Configurations;

namespace MicroLine.Services.Booking.WebApi.Features.Reservations;

internal sealed class ReservationMongoConfigurations : IMongoConfiguration
{
    public void Configure()
    {
        ValueConvertor<ReservationCode, string>.Register(
            reservationCode => reservationCode.ToString(),
            reservationCodeString => ReservationCode.Create(reservationCodeString));
    }
}
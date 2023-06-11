using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Configurations;

namespace MicroLine.Services.Booking.WebApi.Features.Flights
{
    public class FlightMongoConfigurations : IMongoConfiguration
    {
        public void Configure()
        {
            ValueConvertor<FlightNumber, string>.Register(flightNumber => flightNumber.ToString(), FlightNumber.Create);
        }
    }
}

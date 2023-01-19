using MediatR;
using MicroLine.Services.Booking.Domain.Airports;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

namespace MicroLine.Services.Booking.WebApi.Features.Airports;

internal class CreateAirport
{
    public record Command : IRequest<string>
    {
        public required string IcaoCode { get; init; }
        public required string IataCode { get; init; }
        public required string Name { get; init; }
        public required BaseUtcOffsetModel BaseUtcOffset { get; init; }
        public required AirportLocationModel Location { get; init; }


        public record BaseUtcOffsetModel(int Hours, int Minutes);
        public record AirportLocationModel(string Country, string Region, string City);
    }


    public class Handler : IRequestHandler<Command, string>
    {
        private readonly MongoService _mongoService;
        private readonly ILogger<Handler> _logger;

        public Handler(MongoService mongoService, ILogger<Handler> logger)
        {
            _mongoService = mongoService;
            _logger = logger;
        }

        public async Task<string> Handle(Command command, CancellationToken token)
        {
            var icaoCode = IcaoCode.Create(command.IcaoCode);

            var existingAirport = await _mongoService.GetAsync<Airport>(a => a.IcaoCode == icaoCode, token);

            if (existingAirport is not null)
            {
                _logger.LogInformation("An airport with same IcaoCode ({icaoCode}) already exist!", icaoCode);
                return string.Empty;
            }


            var airport = Airport.Create(
                icaoCode,
                command.IataCode,
                command.Name,
                BaseUtcOffset.Create(command.BaseUtcOffset.Hours, command.BaseUtcOffset.Minutes),
                AirportLocation.Create(command.Location.Country, command.Location.Region, command.Location.City)
            );


            _mongoService.Add(airport, token);
            await _mongoService.SaveChangesAsync(token);

            return airport.Id;
        }
    }


}

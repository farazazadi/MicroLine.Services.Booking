using MapsterMapper;
using MediatR;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.WebApi.Features.Passengers.DataTransferObjects;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace MicroLine.Services.Booking.WebApi.Features.Passengers;

internal sealed class GetAllPassengers
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("api/passengers", async (ISender sender, CancellationToken token) =>
        {
            IReadOnlyList<PassengerGroup> passengerGroups = await sender.Send(new Request(), token);
            return Results.Ok(passengerGroups);
        });
    }


    public sealed class PassengerGroup
    {
        public required string RelatedUserExternalId { get; init; }
        public required IReadOnlyList<PassengerDto> Passengers { get; init; }
    }


    public record Request : IRequest<IReadOnlyList<PassengerGroup>>;


    public sealed class RequestHandler : IRequestHandler<Request, IReadOnlyList<PassengerGroup>>
    {
        private readonly MongoService _mongoService;
        private readonly IMapper _mapper;

        public RequestHandler(MongoService mongoService, IMapper mapper)
        {
            _mongoService = mongoService;
            _mapper = mapper;
        }

        public Task<IReadOnlyList<PassengerGroup>> Handle(Request request, CancellationToken token)
        {

            IReadOnlyList<PassengerGroup> passengerGroups = _mongoService
                .GetCollection<Passenger>()
                .AsQueryable()
                .GroupBy(p => p.RelatedUserExternalId)
                .ToList()
                .Select(g => new PassengerGroup
                {
                    RelatedUserExternalId = g.Key.ToString(),
                    Passengers = _mapper.Map<IReadOnlyList<PassengerDto>>(g.ToList())
                })
                .ToList();


            return Task.FromResult(passengerGroups);
        }
    }
}
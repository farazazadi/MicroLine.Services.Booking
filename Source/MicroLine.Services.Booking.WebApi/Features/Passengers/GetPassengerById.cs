using MapsterMapper;
using MediatR;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.WebApi.Common.Exceptions;
using MicroLine.Services.Booking.WebApi.Features.Passengers.DataTransferObjects;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

namespace MicroLine.Services.Booking.WebApi.Features.Passengers;

internal sealed class GetPassengerById
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("api/passengers/{id}", async (string id, ISender sender, CancellationToken token) =>
        {
            PassengerDto passengerDto = await sender.Send(new Request(id), token);

            return Results.Ok(passengerDto);
        });

    }

    public record Request(string Id) : IRequest<PassengerDto>;


    public sealed class RequestHandler : IRequestHandler<Request, PassengerDto>
    {
        private readonly MongoService _mongoService;
        private readonly IMapper _mapper;

        public RequestHandler(MongoService mongoService, IMapper mapper)
        {
            _mongoService = mongoService;
            _mapper = mapper;
        }

        public async Task<PassengerDto> Handle(Request request, CancellationToken token)
        {
            Passenger? passenger = await _mongoService
                .GetAsync<Passenger>(p => p.Id == request.Id, token);

            return _mapper.Map<PassengerDto>(passenger ?? throw new NotFoundException(nameof(Passenger), request.Id));
        }
    }


}
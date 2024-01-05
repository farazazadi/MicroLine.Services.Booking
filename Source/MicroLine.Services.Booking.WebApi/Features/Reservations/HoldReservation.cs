using MediatR;
using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Domain.Reservations;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

namespace MicroLine.Services.Booking.WebApi.Features.Reservations;

internal class HoldReservation
{

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        var url = "api/reservations";

        group.MapPost(url, async (Request request, ISender sender, CancellationToken token) =>
        {
            Response response = await sender.Send(request, token);

            return Results.Created($"{url}/{response.ReservationId}", response);
        });
    }

    public record Request
    (
        string FlightId,
        List<string> PassengersIdList

    ) : IRequest<Response>;


    public record Response
    (
        string ReservationId,
        string ReservationCode
    );

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly MongoService _mongoService;

        public Handler(MongoService mongoService)
        {
            _mongoService = mongoService;
        }

        public async Task<Response> Handle(Request request, CancellationToken token)
        {
            Result validationResult = new();

            Flight? flight = await ValidateAndGetFlight(request.FlightId, validationResult, token);

            IReadOnlyList<Passenger> passengers = await ValidateAndGetPassengers(request.PassengersIdList, validationResult, token);


            if (!validationResult.IsSuccess)
                throw new HoldReservationException(validationResult.GetFailureReasons());

            Reservation reservation = Reservation.Hold(flight!, passengers.ToList());


            _mongoService.Add(reservation, token);

            await _mongoService.SaveChangesAsync(token);

            return new Response(reservation.Id, reservation.ReservationCode);
        }



        private async Task<Flight?> ValidateAndGetFlight(string flightId, Result validationResult, CancellationToken token)
        {
            Flight? flight = await _mongoService
                .GetAsync<Flight>(f => f.Id == flightId, token);

            if (flight is null)
                validationResult.WithFailure($"Flight with Id ({flightId}) does not exist!");

            return flight;
        }


        private async Task<IReadOnlyList<Passenger>> ValidateAndGetPassengers(List<string> passengersIdList, Result validationResult, CancellationToken token)
        {
            IReadOnlyList<Passenger> passengers = await _mongoService
                .GetAllAsync<Passenger>(p => passengersIdList.Contains(p.Id), token);

            if (passengers.Count == passengersIdList.Count)
                return passengers;


            var existingPassengersIds = passengers.Select(p => p.Id.ToString());

            string failureReason = passengersIdList
                .Except(existingPassengersIds)
                .Aggregate(string.Empty, (current, next) =>
                    $"{current}{(current == "" ? "" : Environment.NewLine)}Passenger with Id ({next}) does not exist!");

            validationResult.WithFailure(failureReason);

            return passengers;
        }
    }

}
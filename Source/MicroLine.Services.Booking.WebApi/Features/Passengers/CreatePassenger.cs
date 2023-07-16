using MapsterMapper;
using MediatR;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.WebApi.Features.Passengers.DataTransferObjects;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

namespace MicroLine.Services.Booking.WebApi.Features.Passengers;

internal sealed class CreatePassenger
{
    public static WebApplication MapEndpoint(WebApplication app)
    {
        var baseUrl = "api/passengers";

        app.MapPost(baseUrl, async (Request request, ISender sender, CancellationToken token) =>
        {
            PassengerDto response = await sender.Send(request, token);

            return Results.Created($"{baseUrl}/{response.Id}", response);
        });

        return app;
    }


    public record Request
    (
        string RelatedUserExternalId,
        string NationalId,
        PassportDto Passport,
        string Email,
        string ContactNumber

    ) : IRequest<PassengerDto>;



    public class RequestHandler : IRequestHandler<Request, PassengerDto>
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
            var fullName = FullName.Create(
                request.Passport.FullName.FirstName,
                request.Passport.FullName.LastName);

            var passport = Passport.Create(
                fullName,
                request.Passport.Gender,
                request.Passport.CountryCode,
                request.Passport.PassportNumber,
                request.Passport.BirthDate,
                request.Passport.IssueDate,
                request.Passport.ExpiryDate
            );

            var passenger = Passenger.Create(
                request.RelatedUserExternalId,
                request.NationalId,
                passport,
                request.Email,
                request.ContactNumber
            );


            await EnsureThereIsNoPassengerWithSamePassportAndRelatedUser(passenger, token);

            _mongoService.Add(passenger, token);
            await _mongoService.SaveChangesAsync(token);

            var passengerDto = _mapper.Map<PassengerDto>(passenger);

            return passengerDto;
        }

        private async Task EnsureThereIsNoPassengerWithSamePassportAndRelatedUser(Passenger passenger, CancellationToken token)
        {
            var passengerWithSamePassportAndRelatedUser = await _mongoService.GetAsync<Passenger>(
                p => p.RelatedUserExternalId == passenger.RelatedUserExternalId
                     && p.Passport == passenger.Passport,
                token);

            if (passengerWithSamePassportAndRelatedUser is not null)
                throw new CreatePassengerException("There is already a passenger with same 'Passport' and 'Related User'!");
        }
    }

}
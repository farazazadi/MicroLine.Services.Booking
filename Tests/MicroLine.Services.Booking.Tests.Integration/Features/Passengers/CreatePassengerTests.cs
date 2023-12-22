using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Tests.Common.Fakes;
using MicroLine.Services.Booking.Tests.Common.Fakes.ValueObjects;
using MicroLine.Services.Booking.Tests.Integration.Common;
using MicroLine.Services.Booking.WebApi.Features.Passengers;
using MicroLine.Services.Booking.WebApi.Features.Passengers.DataTransferObjects;

namespace MicroLine.Services.Booking.Tests.Integration.Features.Passengers;

public class CreatePassengerTests : IntegrationTestBase
{
    public CreatePassengerTests(BookingWebApplicationFactory bookingWebApplicationFactory)
        : base(bookingWebApplicationFactory)
    {
    }

    [Fact]
    public async Task CreatePassenger_ShouldCreatedPassengerAsExpected_WhenRequestIsValid()
    {
        // Given
        Passenger passenger = FakePassenger.NewFake();

        var request = Mapper.Map<CreatePassenger.Request>(passenger);

        var expected = Mapper.Map<PassengerDto>(passenger);


        // When
        HttpResponseMessage response = await Client.PostAsJsonAsync("api/passengers", request);


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);


        var passengerDto = await response.Content.ReadFromJsonAsync<PassengerDto>();

        passengerDto!.AuditingDetails.Should().NotBeNull();

        passengerDto.Should().BeEquivalentTo(expected, options =>
        {
            options.Excluding(dto => dto.Id);
            options.Excluding(dto => dto.AuditingDetails);
            return options;
        });


        response.Headers.Location!.ToString().Should().Be($"api/passengers/{passengerDto.Id}");
    }


    [Fact]
    public async Task CreatePassenger_ShouldReturnCreatePassengerProblem_WhenAPassengerWithSamePassportAndRelatedExternalUserAlreadyExist()
    {
        // Given
        Id relatedUserExternalId = Id.Create();

        Passport passport = FakePassport.NewFake();


        Passenger passenger = FakePassenger.NewFake(
            relatedUserExternalId: relatedUserExternalId,
            passport: passport);

        await SaveAsync(passenger);

        var request = Mapper.Map<CreatePassenger.Request>(passenger);


        // When
        HttpResponseMessage response = await Client.PostAsJsonAsync("api/passengers", request);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetailThatContains("There is already a passenger with same 'Passport' and 'Related User'!")
            .WithInstance("/api/passengers")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(CreatePassengerException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);

    }


    [Fact]
    public async Task CreatePassenger_ShouldReturnCreatePassengerProblem_WhenAPassengerWithSameNationalIdAndRelatedExternalUserAlreadyExist()
    {
        // Given
        Id relatedUserExternalId = Id.Create();

        NationalId nationalId = FakeNationalId.NewFake();


        Passenger passenger = FakePassenger.NewFake(
            relatedUserExternalId: relatedUserExternalId,
            nationalId: nationalId);

        await SaveAsync(passenger);

        var request = Mapper.Map<CreatePassenger.Request>(passenger);


        // When
        HttpResponseMessage response = await Client.PostAsJsonAsync("api/passengers", request);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetailThatContains("There is already a passenger with same 'NationalId' and 'Related User'!")
            .WithInstance("/api/passengers")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(CreatePassengerException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);

    }

}
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Tests.Common.Fakes;
using MicroLine.Services.Booking.Tests.Integration.Common;
using MicroLine.Services.Booking.WebApi.Common.Exceptions;
using MicroLine.Services.Booking.WebApi.Features.Passengers.DataTransferObjects;

namespace MicroLine.Services.Booking.Tests.Integration.Features.Passengers;

public class GetPassengerByIdTests : IntegrationTestBase
{
    public GetPassengerByIdTests(BookingWebApplicationFactory bookingWebApplicationFactory) : base(bookingWebApplicationFactory)
    { }

    [Fact]
    public async Task GetPassenger_ShouldReturnPassenger_AsExpected()
    {
        // Given
        Passenger passenger = FakePassenger.NewFake();

        await SaveAsync(passenger);

        PassengerDto expected = Mapper.Map<PassengerDto>(passenger);


        // When
        HttpResponseMessage response = await Client.GetAsync($"/api/passengers/{passenger.Id}");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        PassengerDto? passengerDto = await response.Content.ReadFromJsonAsync<PassengerDto>();

        passengerDto.Should().BeEquivalentTo(expected, options =>
        {
            options.Excluding(dto => dto.AuditingDetails);

            return options;
        });
    }


    [Fact]
    public async Task GetPassenger_ShouldReturnNotFoundProblem_WhenIdIsNotValid()
    {
        // Given
        Id id = Id.Create();


        // When
        HttpResponseMessage response = await Client.GetAsync($"api/passengers/{id}");


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status404NotFound)
            .WithTitle(Constants.Rfc9110.Titles.NotFound)
            .WithDetail($"Passenger with id ({id}) was not found!")
            .WithInstance($"/api/passengers/{id}")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(NotFoundException))
            .WithType(Constants.Rfc9110.StatusCodes.NotFound404);
    }

}
using System.Net;
using System.Net.Http.Json;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Tests.Common.Fakes;
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
    public async Task Passenger_ShouldBeCreatedAsExpected_WhenRequestIsValid()
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


        response.Headers.Location!.ToString().Should().Be($"api/passengers/{passengerDto!.Id}");
    }

}
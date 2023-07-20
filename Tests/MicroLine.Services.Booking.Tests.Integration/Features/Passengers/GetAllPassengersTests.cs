using System.Net;
using System.Net.Http.Json;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Tests.Common.Fakes;
using MicroLine.Services.Booking.Tests.Integration.Common;
using MicroLine.Services.Booking.WebApi.Features.Passengers;
using MicroLine.Services.Booking.WebApi.Features.Passengers.DataTransferObjects;

namespace MicroLine.Services.Booking.Tests.Integration.Features.Passengers;

public class GetAllPassengersTests : IntegrationTestBase
{
    public GetAllPassengersTests(BookingWebApplicationFactory bookingWebApplicationFactory) : base(bookingWebApplicationFactory)
    { }

    [Fact]
    public async Task AllPassengers_ShouldBeReturnedAsExpected_WhenAllPassengersHaveSameRelatedUserExternalId()
    {
        // Given
        await BookingWebApplicationFactory.ResetDatabaseAsync();

        await SaveAsync(FakePassenger.NewFakeList(5));

        IReadOnlyList<Passenger> passengers = await GetAllAsync<Passenger>();


        var expected = Mapper.Map<List<PassengerDto>>(passengers)
            .GroupBy(p => p.RelatedUserExternalId)
            .Select(g => new GetAllPassengers.PassengerGroup
            {
                RelatedUserExternalId = g.Key,
                Passengers = g.ToList()
            })
            .ToList();

        // When
        HttpResponseMessage response = await Client.GetAsync("api/passengers");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var passengerGroups = await response.Content.ReadFromJsonAsync<IReadOnlyList<GetAllPassengers.PassengerGroup>>();

        passengerGroups.Should().BeEquivalentTo(expected);
    }



    [Fact]
    public async Task AllPassengers_ShouldBeReturnedAsExpected_WhenAllPassengersDoNotHaveSameRelatedUserExternalId()
    {
        // Given
        await BookingWebApplicationFactory.ResetDatabaseAsync();

        List<Passenger> passengersRelatedToFirstUser = FakePassenger.NewFakeList(3, relatedUserExternalId: Id.Create());
        List<Passenger> passengersRelatedToSecondUser = FakePassenger.NewFakeList(2, relatedUserExternalId: Id.Create());

        await SaveAsync(passengersRelatedToFirstUser, passengersRelatedToSecondUser);

        IReadOnlyList<Passenger> passengers = await GetAllAsync<Passenger>();


        var expected = Mapper.Map<List<PassengerDto>>(passengers)
            .GroupBy(p => p.RelatedUserExternalId)
            .Select(g => new GetAllPassengers.PassengerGroup
            {
                RelatedUserExternalId = g.Key,
                Passengers = g.ToList()
            })
            .ToList();

        // When
        HttpResponseMessage response = await Client.GetAsync("api/passengers");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var passengerGroups = await response.Content.ReadFromJsonAsync<IReadOnlyList<GetAllPassengers.PassengerGroup>>();

        passengerGroups.Should().BeEquivalentTo(expected);
    }
}
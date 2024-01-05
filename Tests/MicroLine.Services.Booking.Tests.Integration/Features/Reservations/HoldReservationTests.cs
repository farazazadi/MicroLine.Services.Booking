using MicroLine.Services.Booking.Domain.Flights;
using MicroLine.Services.Booking.Domain.Passengers;
using MicroLine.Services.Booking.Domain.Reservations;
using MicroLine.Services.Booking.Tests.Common.Fakes;
using MicroLine.Services.Booking.Tests.Integration.Common;
using MicroLine.Services.Booking.WebApi.Features.Reservations;


namespace MicroLine.Services.Booking.Tests.Integration.Features.Reservations;

public class HoldReservationTests : IntegrationTestBase
{
    public HoldReservationTests(BookingWebApplicationFactory bookingWebApplicationFactory)
        : base(bookingWebApplicationFactory)
    {
    }


    [Fact]
    public async Task HoldReservation_ShouldCreateReservationInHoldState_WhenRequestIsValid()
    {
        // Given
        Flight flight = FakeFlight.NewFake();
        List<Passenger> passengers = FakePassenger.NewFakeList(3);

        await SaveAsync(flight);
        await SaveAsync(passengers);

        var passengersIdList = passengers.Select(p => p.Id.ToString()).ToList();

        var request = new HoldReservation.Request(flight.Id, passengersIdList);


        // When
        HttpResponseMessage response = await Client.PostAsJsonAsync("api/reservations", request);


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        HoldReservation.Response? holdReservationResponse =
            await response.Content.ReadFromJsonAsync<HoldReservation.Response>();
        response.Headers.Location?.ToString().Should().Be($"api/reservations/{holdReservationResponse!.ReservationId}");


        Reservation? reservation = await FindAsync<Reservation>(r => r.Id == holdReservationResponse!.ReservationId);
        reservation.Should().NotBeNull();

        reservation!.ReservationCode.ToString().Should().Be(holdReservationResponse!.ReservationCode);

        reservation.Passengers.Should().BeEquivalentTo(passengers);
        reservation.Flight.Id.Should().Be(flight.Id);
    }


    [Fact]
    public async Task HoldReservation_ShouldReturnHoldReservationProblem_WhenFlightDoesNotExist()
    {
        // Given
        Flight flight = FakeFlight.NewFake();

        List<Passenger> passengers = FakePassenger.NewFakeList(2);
        var passengersIdList = passengers.Select(p => p.Id.ToString()).ToList();

        await SaveAsync(passengers);

        var request = new HoldReservation.Request(flight.Id, passengersIdList);


        // When
        HttpResponseMessage response = await Client.PostAsJsonAsync("api/reservations", request);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"Flight with Id ({flight.Id}) does not exist!")
            .WithInstance("/api/reservations")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(HoldReservationException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }


    [Fact]
    public async Task HoldReservation_ShouldReturnHoldReservationProblem_WhenAnyOfPassengersDoesNotExist()
    {
        // Given
        Flight flight = FakeFlight.NewFake();
        await SaveAsync(flight);

        Passenger firstPassenger = FakePassenger.NewFake();
        Passenger secondPassenger = FakePassenger.NewFake();

        List<string> passengersIdList = new()
        {
            firstPassenger.Id,
            secondPassenger.Id
        };

        await SaveAsync(firstPassenger);

        var request = new HoldReservation.Request(flight.Id, passengersIdList);


        // When
        HttpResponseMessage response = await Client.PostAsJsonAsync("api/reservations", request);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"Passenger with Id ({secondPassenger.Id}) does not exist!")
            .WithInstance("/api/reservations")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(HoldReservationException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }


}
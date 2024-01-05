namespace MicroLine.Services.Booking.WebApi.Features.Reservations;

internal static class EndpointExtensions
{
    public static WebApplication MapReservationsEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup(string.Empty)
            .WithTags("Reservations");

        HoldReservation.MapEndpoint(group);

        return app;
    }
}
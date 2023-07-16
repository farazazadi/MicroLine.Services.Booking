namespace MicroLine.Services.Booking.WebApi.Features.Passengers;

internal static class EndpointExtensions
{
    public static WebApplication MapPassengerEndpoints(this WebApplication app)
    { 
        CreatePassenger.MapEndpoint(app);

        return app;
    }
}
namespace MicroLine.Services.Booking.WebApi.Features.Passengers;

internal static class EndpointExtensions
{
    public static WebApplication MapPassengerEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app
            .MapGroup(string.Empty)
            .WithTags("Passengers");
        
        CreatePassenger.MapEndpoint(group);
        GetPassengerById.MapEndpoint(group);
        GetAllPassengers.MapEndpoint(group);

        return app;
    }
}
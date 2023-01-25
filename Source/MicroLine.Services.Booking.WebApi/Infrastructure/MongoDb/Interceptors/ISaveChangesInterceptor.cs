namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Interceptors;

internal interface ISaveChangesInterceptor
{
    Task SavingChangesAsync(MongoService mongoService, CancellationToken cancellationToken = default);
    Task SavedChangesAsync(MongoService mongoService, CancellationToken cancellationToken = default);
}
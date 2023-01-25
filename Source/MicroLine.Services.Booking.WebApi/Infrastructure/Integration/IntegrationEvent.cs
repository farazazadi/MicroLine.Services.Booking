using MediatR;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.Integration;

internal abstract class IntegrationEvent : INotification
{
    public Guid EventId { get; } = Guid.NewGuid();
    public abstract string EventName { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
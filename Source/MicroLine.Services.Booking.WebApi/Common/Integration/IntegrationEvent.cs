using MediatR;

namespace MicroLine.Services.Booking.WebApi.Common.Integration;

public abstract class IntegrationEvent : INotification
{
    public Guid EventId { get; } = Guid.NewGuid();
    public abstract string EventName { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
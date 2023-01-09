using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Domain.Common;

public abstract class DomainEvent
{
    public Id Id { get; } = Id.Create();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.Now;

}
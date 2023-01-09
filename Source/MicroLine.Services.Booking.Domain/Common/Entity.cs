using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;

namespace MicroLine.Services.Booking.Domain.Common;

public abstract class Entity
{
    public Id Id { get; protected set; } = Id.Create();

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (this.GetRealType() != other.GetRealType())
            return false;

        if (IsTransient() || other.IsTransient())
            return false;

        return Id.Equals(other.Id);
    }


    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    private bool IsTransient() => Id.IsTransient;

    public static bool operator !=(Entity a, Entity b) => !(a == b);

    public override int GetHashCode() => (this.GetRealType() + Id).GetHashCode();
}

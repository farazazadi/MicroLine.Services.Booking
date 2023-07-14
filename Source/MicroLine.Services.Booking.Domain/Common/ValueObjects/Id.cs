using MicroLine.Services.Booking.Domain.Common.Exceptions;

namespace MicroLine.Services.Booking.Domain.Common.ValueObjects;

public sealed class Id : ValueObject
{
    public bool IsTransient => Value == string.Empty;
    public string Value { get; }

    public static Id Transient => new(string.Empty);

    private Id(string id) => Value = id;

    public static Id Create() => Create(Guid.NewGuid().ToString());

    public static Id Create(string id)
    {
        Validate(id);

        return new Id(id);
    }

    private static void Validate(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new InvalidIdException($"Id ({id}) is not valid!");
    }


    public static implicit operator string(Id id) => id.Value;

    public static implicit operator Id(string id) => new(id);

    public override string ToString() => Value;
}
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Serializers;

internal sealed class MongoDateOnlySerializer : StructSerializerBase<DateOnly>
{
    private readonly DateTimeSerializer _innerSerializer = DateTimeSerializer.DateOnlyInstance;

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
    {
        _innerSerializer
            .Serialize(context, args, value.ToDateTime(TimeOnly.MinValue));
    }

    public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var dateTime = _innerSerializer.Deserialize(context, args);
        return DateOnly.FromDateTime(dateTime);
    }
}
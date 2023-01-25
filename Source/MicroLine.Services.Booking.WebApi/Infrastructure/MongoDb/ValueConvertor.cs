using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

internal class ValueConvertor<TSource, TDest> : SerializerBase<TSource>
{
    private readonly Func<TSource, TDest> _convertTo;
    private readonly Func<TDest, TSource> _convertFrom;
    private readonly IBsonSerializer<TDest> _serializer;
    public ValueConvertor(Func<TSource, TDest> convertToProvider,
        Func<TDest, TSource> convertFromProvider)
    {
        _convertTo = convertToProvider;
        _convertFrom = convertFromProvider;

        _serializer = BsonSerializer.LookupSerializer<TDest>();
    }

    public override TSource Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        => _convertFrom.Invoke(_serializer.Deserialize(context, args));

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TSource source)
        => _serializer.Serialize(context, args, _convertTo.Invoke(source));

    public static void Register(
        Func<TSource, TDest> convertToProvider,
        Func<TDest, TSource> convertFromProvider)
    {
        var valueConvertor = new ValueConvertor<TSource, TDest>(convertToProvider, convertFromProvider);
        BsonSerializer.RegisterSerializer(valueConvertor);
    }
}
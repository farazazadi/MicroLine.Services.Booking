using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Conventions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Configurations;

internal class GeneralMongoConfigurations : IMongoConfiguration
{
    public void Configure()
    {
        BsonSerializer.RegisterSerializer(
            typeof(decimal),
            new DecimalSerializer(BsonType.Decimal128)
        );

        BsonSerializer.RegisterSerializer(
            typeof(Guid),
            new GuidSerializer(BsonType.String)
        );

        ValueConvertor<Id, string>.Register(id => id, idString => idString);

        RegisterConventions();
    }

    private void RegisterConventions()
    {
        var conventionPack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
            new CamelCaseElementNameConvention(),
            new EnumRepresentationConvention(BsonType.String),
            new ImmutableClassMapConvention()
        };


        ConventionRegistry.Register("conventions", conventionPack, _ => true);
    }
}

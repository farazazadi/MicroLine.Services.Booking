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

        RegisterCommonValueObjectsConvertors();

        RegisterConventions();
    }

    private static void RegisterCommonValueObjectsConvertors()
    {
        ValueConvertor<Id, string>.Register(
            id => id.ToString(),
            idString => Id.Create(idString));

        ValueConvertor<Date, DateOnly>.Register(
            date => (DateOnly)date,
            dateOnly => (Date)dateOnly);

        ValueConvertor<Email, string>.Register(
            email => email.ToString(),
            emailString => Email.Create(emailString));
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

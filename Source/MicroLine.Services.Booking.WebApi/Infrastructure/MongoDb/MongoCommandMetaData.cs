namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

internal sealed class MongoCommandMetaData
{
    public MongoCommandType CommandType { get; }
    public IEnumerable<dynamic> Entities { get; }
    public MongoCommandMetaData(MongoCommandType type, IEnumerable<dynamic> entities)
    {
        CommandType = type;
        Entities = entities;
    }

}
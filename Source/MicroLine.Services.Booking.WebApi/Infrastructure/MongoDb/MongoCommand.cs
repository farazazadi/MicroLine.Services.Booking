
namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

internal sealed class MongoCommand
{
    public MongoCommandMetaData MetaData { get;}
    public Func<Task> Command {get;}

    public MongoCommand(MongoCommandType type, IEnumerable<dynamic> entities, Func<Task> command)
    {
        MetaData = new MongoCommandMetaData(type, entities);

        Command = command;
    }

}
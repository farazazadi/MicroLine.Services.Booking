using MicroLine.Services.Booking.Domain.Common;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Interceptors;

internal class AuditingInterceptor : ISaveChangesInterceptor
{
    public Task SavingChangesAsync(MongoService service, CancellationToken cancellationToken = default)
    {

        foreach (var data in service.QueuedCommandsMetaData)
        {
            foreach (var entity in data.Entities)
            {
                if (entity is not AuditableEntity auditableEntity)
                    continue;

                Audit(auditableEntity, data.CommandType);
            }

        }

        return Task.CompletedTask;
    }

    public Task SavedChangesAsync(MongoService service, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }


    private void Audit(AuditableEntity entity, MongoCommandType commandType)
    {
        var utcNow = DateTime.UtcNow;

        if (commandType is MongoCommandType.Add)
        {
            entity.SetCreationDetails("", utcNow);
            entity.SetModificationDetails("", utcNow);
            return;
        }

        entity.SetModificationDetails("", utcNow);
    }
}
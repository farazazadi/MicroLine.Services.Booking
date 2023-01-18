using System.Collections.Concurrent;
using MongoDB.Driver;
using System.Linq.Expressions;
using Humanizer;
using Microsoft.Extensions.Options;
using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Interceptors;
using MongoDB.Driver.Linq;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

internal sealed class MongoService
{
    private readonly IEnumerable<ISaveChangesInterceptor> _saveChangesInterceptors;
    private readonly ILogger<MongoService> _logger;
    private readonly MongoClient _client;

    private readonly ConcurrentDictionary<string, dynamic> _collectionsCache = new();

    private readonly ConcurrentQueue<MongoCommand> _commandsQueue = new();

    public bool IsTransactionSupported { get; }

    public  IMongoDatabase Database { get; }

    public IReadOnlyList<MongoCommandMetaData> QueuedCommandsMetaData
        => _commandsQueue.Select(command => command.MetaData).ToList();

    public MongoService(
        IOptions<MongoDbOptions> options,
        IEnumerable<ISaveChangesInterceptor> saveChangesInterceptors,
        ILogger<MongoService> logger)
    {

        var optionsValue = options.Value;

        _saveChangesInterceptors = saveChangesInterceptors;

        _logger = logger;

        var settings = MongoClientSettings
            .FromConnectionString(optionsValue.ConnectionString);
        settings.LinqProvider = LinqProvider.V3;

        _client = new MongoClient(settings);

        Database = _client.GetDatabase(optionsValue.DatabaseName);

        IsTransactionSupported = !optionsValue.StandaloneServer;
    }



    public IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : Entity
    {
        var collectionName = typeof(TEntity).Name
            .Pluralize(inputIsKnownToBeSingular: false)
            .ToLowerInvariant();

        return _collectionsCache
            .GetOrAdd(collectionName, _ => Database.GetCollection<TEntity>(collectionName));
    }



    public async Task<IReadOnlyList<TEntity>> GetAllAsync<TEntity>(CancellationToken token = default)
        where TEntity : AggregateRoot
    {
        var collection = GetCollection<TEntity>();

        var result = await collection.FindAsync(_ => true, cancellationToken: token);

        return await result.ToListAsync(token);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        where TEntity : AggregateRoot
    {
        var collection = GetCollection<TEntity>();

        var result = await collection.FindAsync(predicate, cancellationToken: token);

        return await result.ToListAsync(token);
    }

    public async Task<TEntity?> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        where TEntity : AggregateRoot
    {
        var collection = GetCollection<TEntity>();

        var result = await collection
            .Find(predicate)
            .Limit(1)
            .FirstOrDefaultAsync(token);

        return result;
    }

    public async Task<TEntity?> GetByIdAsync<TEntity>(Id id, CancellationToken token = default)
        where TEntity : AggregateRoot
    {
        var collection = GetCollection<TEntity>();

        var result = await collection
            .Find(entity => entity.Id == id)
            .Limit(1)
            .SingleOrDefaultAsync(token);

        return result;
    }


    public void Add<TEntity>(TEntity entity, CancellationToken token = default)
        where TEntity : AggregateRoot
    {
        Task Command()
        {
            var collection = GetCollection<TEntity>();

            var options = new InsertOneOptions {BypassDocumentValidation = false};

            return collection.InsertOneAsync(entity, options, token);
        }

        _commandsQueue.Enqueue(new MongoCommand(
            MongoCommandType.Add,
            new []{ entity },
            Command
        ));
    }

    public void AddRange<TEntity>(IEnumerable<TEntity> entities, CancellationToken token = default)
        where TEntity : AggregateRoot
    {
        var entityList = entities.ToList();

        Task Command()
        {
            var collection = GetCollection<TEntity>();

            var options = new InsertManyOptions {BypassDocumentValidation = false};

            return collection.InsertManyAsync(entityList, options, token);
        }

        _commandsQueue.Enqueue(new MongoCommand(
            MongoCommandType.Add,
            entityList,
            Command
            ));
    }


    public void Update<TEntity>(TEntity entity, CancellationToken token = default)
        where TEntity : AggregateRoot
    {

        Task<ReplaceOneResult> Command()
        {
            var collection = GetCollection<TEntity>();

            var replaceOptions = new ReplaceOptions {IsUpsert = false,};

            return collection.ReplaceOneAsync(document => document.Id == entity.Id, entity, replaceOptions, token);
        }

        _commandsQueue.Enqueue(new MongoCommand(
            MongoCommandType.Update,
            new[] { entity },
            Command
        ));
    }


    public void Delete<TEntity>(TEntity entity, CancellationToken token = default)
        where TEntity : AggregateRoot
    {
        Task<DeleteResult> Command()
        {
            var collection = GetCollection<TEntity>();
            return collection.DeleteOneAsync(document => document.Id == entity.Id, token);
        }

        _commandsQueue.Enqueue(new MongoCommand(
            MongoCommandType.Delete,
            new[] { entity },
            Command
            ));
    }


    public async Task SaveChangesAsync(CancellationToken token = default)
    {
        if (_commandsQueue.IsEmpty)
            return;

        if (!IsTransactionSupported)
        {
            await SaveChangesInternalAsync(token);
            return;
        }

        using var session = await _client.StartSessionAsync(cancellationToken: token);

        session.StartTransaction();

        try
        {
            await SaveChangesInternalAsync(token);

            await session.CommitTransactionAsync(token);
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync(token);
            _logger.LogError(ex, "The saving changes process in {BoundedContext} bounded context failed and all changes rolled back!", "Booking");
            throw;
        }

    }

    private async Task SaveChangesInternalAsync(CancellationToken token = default)
    {
        foreach (var interceptor in _saveChangesInterceptors)
            await interceptor.SavingChangesAsync(this, token);

        while (_commandsQueue.TryDequeue(out var command))
            await command.Command();

        foreach (var interceptor in _saveChangesInterceptors)
            await interceptor.SavedChangesAsync(this, token);
    }

}
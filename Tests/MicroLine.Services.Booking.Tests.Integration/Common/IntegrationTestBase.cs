using System.Linq.Expressions;
using MapsterMapper;
using MediatR;
using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq.Publisher;
using Microsoft.Extensions.DependencyInjection;

namespace MicroLine.Services.Booking.Tests.Integration.Common;


[Collection(nameof(BookingWebApplicationFactoryCollection))]
public abstract class IntegrationTestBase
{
    protected readonly BookingWebApplicationFactory BookingWebApplicationFactory;
    protected readonly HttpClient Client;
    protected readonly IMapper Mapper;
    private readonly MongoService _mongoService;
    private readonly ISender _sender;
    private protected readonly RabbitMqPublisher RabbitMqPublisher;

    protected const string AirlineExchangeName = "Airline_Test";

    protected IntegrationTestBase(BookingWebApplicationFactory bookingWebApplicationFactory)
    {
        BookingWebApplicationFactory = bookingWebApplicationFactory;

        Client = BookingWebApplicationFactory.CreateClient();

        Mapper = BookingWebApplicationFactory.Services.GetRequiredService<IMapper>();

        _mongoService = BookingWebApplicationFactory.Services.GetRequiredService<MongoService>();

        _sender = bookingWebApplicationFactory.Services.GetRequiredService<ISender>();

        RabbitMqPublisher = bookingWebApplicationFactory.Services.GetRequiredService<RabbitMqPublisher>();
    }

    protected Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : AggregateRoot
        => _mongoService.GetAsync(predicate);

    protected Task<TEntity?> FindByIdAsync<TEntity>(Id id) where TEntity : AggregateRoot
        => _mongoService.GetByIdAsync<TEntity>(id);

    protected Task<IReadOnlyList<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : AggregateRoot
        => _mongoService.GetAllAsync(predicate);

    protected Task<IReadOnlyList<TEntity>> GetAllAsync<TEntity>() where TEntity : AggregateRoot
        => _mongoService.GetAllAsync<TEntity>();

    protected Task SaveAsync<TEntity>(TEntity entity) where TEntity : AggregateRoot
    {
        _mongoService.Add(entity);
        return _mongoService.SaveChangesAsync();
    }

    protected Task SaveAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : AggregateRoot
    {
        _mongoService.AddRange(entities);
        return _mongoService.SaveChangesAsync();
    }

    protected Task SaveAsync<TEntity>(params IEnumerable<TEntity>[] entitiesList) where TEntity : AggregateRoot
    {
        foreach (var entities in entitiesList)
            _mongoService.AddRange(entities);

        return _mongoService.SaveChangesAsync();
    }


    public async Task<TEntity> WaitUntilGetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        int delayInterval = 1000, CancellationToken token = default)
        where TEntity : AggregateRoot
    {
        // Unfortunately, 'Change Streams' feature is not available for MongoDB standalone servers.

        TEntity? entity = null;

        while (entity is null)
        {
            entity = await _mongoService.GetAsync(predicate, token);

            if (entity is null)
                await Task.Delay(delayInterval, token);
        }

        return entity;
    }


    protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request) =>
        await _sender.Send(request);

}

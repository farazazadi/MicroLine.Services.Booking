using System.Linq.Expressions;
using MapsterMapper;
using MediatR;
using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
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

    protected IntegrationTestBase(BookingWebApplicationFactory bookingWebApplicationFactory)
    {
        BookingWebApplicationFactory = bookingWebApplicationFactory;

        Client = BookingWebApplicationFactory.CreateClient();

        Mapper = BookingWebApplicationFactory.Services.GetRequiredService<IMapper>();

        _mongoService = BookingWebApplicationFactory.Services.GetRequiredService<MongoService>();

        _sender = bookingWebApplicationFactory.Services.GetRequiredService<ISender>();
    }

    protected Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : AggregateRoot
        => _mongoService.GetAsync<TEntity>(predicate);

    protected Task<TEntity?> FindByIdAsync<TEntity>(Id id) where TEntity : AggregateRoot
        => _mongoService.GetByIdAsync<TEntity>(id);

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



    protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request) =>
        await _sender.Send(request);

}

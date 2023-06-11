using MediatR;
using MicroLine.Services.Booking.WebApi.Common.Integration;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.Inbox;

internal sealed class InboxProcessor : BackgroundService
{
    private readonly IPublisher _publisher;
    private readonly MongoService _mongoService;
    private readonly ILogger<InboxProcessor> _logger;
    private readonly PeriodicTimer _periodicTimer;

    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

    public InboxProcessor(
        IOptions<InboxProcessorOptions> options,
        IPublisher publisher,
        MongoService mongoService,
        ILogger<InboxProcessor> logger)
    {
        _publisher = publisher;
        _mongoService = mongoService;
        _logger = logger;

        var option = options.Value;

        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(option.AllowedExceptionsCountBeforeBreaking,
                TimeSpan.FromSeconds(option.DurationOfBreakInSeconds));

        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(option.ProcessingIntervalInSeconds));
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {

        while (!token.IsCancellationRequested &&
               await _periodicTimer.WaitForNextTickAsync(token))
        {
            if (_circuitBreakerPolicy.CircuitState is CircuitState.Open or CircuitState.Isolated)
                continue;

            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(() =>  ProcessAsync(token));
            }
            catch
            {
                // ignored
            }
        }
    }

    public async Task ProcessAsync(CancellationToken token)
    {
        var inboxMessages = await _mongoService
                .GetAllAsync<InboxMessage>(message => message.Processed == false, token);

        if (!inboxMessages.Any())
            return;


        foreach (var message in inboxMessages)
        {
            try
            {
                var integrationEvent = message.ToIntegrationEvent();

                if(integrationEvent is null)
                    continue;

                await _publisher.Publish(integrationEvent, token);

                message.Process();

                _mongoService.Update(message, token);

                await _mongoService.SaveChangesAsync(token);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The InboxMessage with Id '{MessageId}' and '{MessageSubject}' could not be processed!",
                    message.Id, message.Subject);

                throw;
            }
        }

    }

}

using MicroLine.Services.Booking.Domain.Common.Extensions;
using MicroLine.Services.Booking.WebApi.Infrastructure.Inbox;
using MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq.Subscriber;

internal sealed class RabbitMqSubscriber : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IConnection? _connection;
    private IModel? _channel;
    private readonly List<string> _queuesName;

    private readonly AsyncRetryPolicy _asyncRetryPolicy;

    public RabbitMqSubscriber(
        IOptions<RabbitMqOptions> rabbitMqOptions,
        IOptions<RabbitMqSubscriberOptions> rabbitMqSubscriberOptions,
        IServiceScopeFactory serviceScopeFactory)
    {

        var options = rabbitMqOptions.Value;

        //todo: bindings props should be checked against being null or empty
        var bindings = rabbitMqSubscriberOptions.Value.Bindings;

        _queuesName = bindings!.Select(binding => binding.QueueName).ToList();


        var connectionFactory = new ConnectionFactory
        {
            HostName = options.HostName,
            Port = options.Port,
            UserName = options.UserName,
            Password = options.Password,
            VirtualHost = options.VirtualHost,
            AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled,
            DispatchConsumersAsync = true
        };


        var delays = Backoff.DecorrelatedJitterBackoffV2(
                TimeSpan.FromSeconds(options.BackOffFirstRetryDelayInSeconds),
                options.RetryCountOnFailure)
            .ToList();

        
        Policy
            .Handle<Exception>()
            .WaitAndRetry(delays)
            .Execute(() =>
            {
                _connection = connectionFactory.CreateConnection(options.ClientProvidedName);
                _channel = _connection.CreateModel();
                _channel.BasicQos(0, 1, true);

                foreach (var binding in bindings!)
                {
                    _channel.ExchangeDeclare(binding.ExchangeName, ExchangeType.Fanout, durable:true);
                    _channel.QueueDeclare(binding.QueueName, durable: true, exclusive: false, autoDelete: false);
                    _channel.QueueBind(binding.QueueName, binding.ExchangeName, routingKey: string.Empty);
                }
            });

        _asyncRetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(delays);

        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (_, args) =>
        {
            try
            {
                await _asyncRetryPolicy.ExecuteAsync(() => ProcessAsync(args, token));
            }
            catch (Exception)
            {
                _channel?.BasicNack(args.DeliveryTag, false, true);
            }
        };

        foreach (var queueName in _queuesName)
            _channel.BasicConsume(queueName, autoAck: false, consumerTag: queueName, consumer);


        await Task.CompletedTask;
    }

    private async Task ProcessAsync(BasicDeliverEventArgs args, CancellationToken token)
    {
        if (!IsValidMessage(args))
        {
            _channel?.BasicNack(args.DeliveryTag, false, false);
            return;
        }

        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var mongoService = scope.ServiceProvider.GetRequiredService<MongoService>();

        var messageAlreadyExistInInbox = await MessageAlreadyExistInInbox(args, mongoService, token);

        if (messageAlreadyExistInInbox)
        {
            _channel?.BasicAck(args.DeliveryTag, false);
            return;
        }


        await AddTheMessageToTheInbox(args, mongoService, token);

        _channel?.BasicAck(args.DeliveryTag, false);
    }


    private static bool IsValidMessage(BasicDeliverEventArgs args)
    {
        var messageId = args.BasicProperties.MessageId;
        var subject = args.BasicProperties.Type;

        return !messageId.IsNullOrWhiteSpace() && !subject.IsNullOrWhiteSpace();
    }

    private async Task<bool> MessageAlreadyExistInInbox(BasicDeliverEventArgs args, MongoService mongoService,
        CancellationToken token)
    {
        var messageId = args.BasicProperties.MessageId;

        var message = await mongoService.GetByIdAsync<InboxMessage>(messageId, token);

        return message is not null;
    }

    private async Task AddTheMessageToTheInbox(BasicDeliverEventArgs args, MongoService mongoService,
        CancellationToken token)
    {
        var messageId = args.BasicProperties.MessageId;
        var subject = args.BasicProperties.Type;
        var content = args.Body.ReadAsString();

        var inboxMessage = InboxMessage.Create(messageId, subject, content);

        mongoService.Add(inboxMessage, token);

        await mongoService.SaveChangesAsync(token);
    }


    public override void Dispose()
    {
        if (_channel is { IsOpen: true })
        {
            _channel.Close();
            _channel.Dispose();
        }

        if (_connection is { IsOpen: true })
        {
            _connection.Close();
            _connection.Dispose();
        }

        base.Dispose();
    }
}
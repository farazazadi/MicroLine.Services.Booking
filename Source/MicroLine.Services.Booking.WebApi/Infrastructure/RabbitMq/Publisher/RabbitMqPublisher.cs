using MicroLine.Services.Booking.WebApi.Common.Integration;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using RabbitMQ.Client;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq.Publisher;

internal class RabbitMqPublisher : IDisposable
{
    private readonly RabbitMqOptions _rabbitMqOptions;

    private IConnection? _connection;
    private IModel? _channel;
    private readonly string _exchangeName;
    private readonly string _routingKey = string.Empty;

    private readonly RetryPolicy _retryPolicy;

    public RabbitMqPublisher(
        IOptions<RabbitMqOptions> rabbitMqOptions,
        IOptions<RabbitMqPublisherOptions> rabbitMqPublisherOptions
        )
    {
        _rabbitMqOptions = rabbitMqOptions.Value;
        var publisherOptions = rabbitMqPublisherOptions.Value;

        _exchangeName = publisherOptions.ExchangeName!;
        
        var delays = Backoff.DecorrelatedJitterBackoffV2(
            TimeSpan.FromSeconds(_rabbitMqOptions.BackOffFirstRetryDelayInSeconds),
            _rabbitMqOptions.RetryCountOnFailure);

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(delays);
    }

    private void CreateAndEstablishConnection()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.HostName,
            Port = _rabbitMqOptions.Port,
            UserName = _rabbitMqOptions.UserName,
            Password = _rabbitMqOptions.Password,
            VirtualHost = _rabbitMqOptions.VirtualHost,
            AutomaticRecoveryEnabled = _rabbitMqOptions.AutomaticRecoveryEnabled,
            DispatchConsumersAsync = true
        };


        _connection = connectionFactory.CreateConnection(_rabbitMqOptions.ClientProvidedName);
    }

    private void CreateChannel()
    {
        _channel = _connection?.CreateModel();
    }

    private void DeclareExchange(string? exchangeName = null)
    {
        _channel?.ExchangeDeclare(exchangeName ?? _exchangeName, ExchangeType.Fanout, true);
    }


    private void Prepare(string? exchangeName = null)
    {
        if (_channel is not null) return;

        CreateAndEstablishConnection();
        CreateChannel();
        DeclareExchange(exchangeName);
    }


    public void Publish<TIntegrationEvent>(TIntegrationEvent integrationEvent, string? exchangeName = null) where TIntegrationEvent : IntegrationEvent
    {
        var id = integrationEvent.EventId.ToString();
        var subject = typeof(TIntegrationEvent).Name;
        var content = integrationEvent.ToJsonString();

        Publish(id, subject, content, exchangeName);
    }

    public void Publish(string id, string subject, string content, string? exchangeName = null)
    {
        var contentBytes = content.ToByteArray();

        Publish(id, subject, contentBytes, exchangeName);
    }

    private void Publish(string id, string subject, byte[] content, string? exchangeName = null)
    {
        _retryPolicy.Execute(() =>
        {
            Prepare(exchangeName);

            var properties = _channel!.CreateBasicProperties();

            properties.MessageId = id;
            properties.Type = subject;

            _channel.BasicPublish(exchangeName ?? _exchangeName, _routingKey, body: content, basicProperties: properties);
        });
    }


    public void Dispose()
    {
        if (_channel is {IsOpen: true})
        {
            _channel.Close();
            _channel.Dispose();
        }

        if (_connection is {IsOpen: true})
        {
            _connection.Close();
            _connection?.Dispose();
        }
    }
}

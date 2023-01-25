using MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq.Publisher;
using MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq.Subscriber;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq;

internal static class DependencyInjection
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services
            .AddOptions<RabbitMqOptions>()
            .BindConfiguration(RabbitMqOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();


        services
            .AddOptions<RabbitMqPublisherOptions>()
            .BindConfiguration(RabbitMqPublisherOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<RabbitMqPublisher>();



        services
            .AddOptions<RabbitMqSubscriberOptions>()
            .BindConfiguration(RabbitMqSubscriberOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHostedService<RabbitMqSubscriber>();

        return services;
    }

}

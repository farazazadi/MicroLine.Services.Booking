using System.ComponentModel.DataAnnotations;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq.Subscriber;

public class RabbitMqSubscriberOptions
{
    public static string SectionName => "RabbitMq:Subscriber";

    [Required]
    public List<QueueBindingModel>? Bindings { get; set; }

    public class QueueBindingModel
    {
        public string ExchangeName { get; set; } = string.Empty;

        public string QueueName { get; set; } = string.Empty;
    }
}


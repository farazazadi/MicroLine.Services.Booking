using System.ComponentModel.DataAnnotations;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq.Publisher;

public class RabbitMqPublisherOptions
{
    public static string SectionName => "RabbitMq:Publisher";

    [Required]
    public string? ExchangeName { get; set; }

}
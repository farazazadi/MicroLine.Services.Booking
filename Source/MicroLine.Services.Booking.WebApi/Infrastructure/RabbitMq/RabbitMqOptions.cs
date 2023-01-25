using System.ComponentModel.DataAnnotations;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.RabbitMq;
internal class RabbitMqOptions
{
    public static string SectionName => "RabbitMq";

    [Required]
    public string? ClientProvidedName { get; set; }

    [Required]
    public string? HostName { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    public string? VirtualHost { get; set; }

    [Required]
    public bool AutomaticRecoveryEnabled { get; set; }

    [Required]
    public byte RetryCountOnFailure { get; set; }

    [Required]
    public int BackOffFirstRetryDelayInSeconds { get; set; }
}
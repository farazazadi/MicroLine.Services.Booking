using System.ComponentModel.DataAnnotations;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.Inbox;

internal class InboxProcessorOptions
{
    public static string SectionName => "InboxProcessor";

    [Required]
    public int ProcessingIntervalInSeconds { get; set; }

    [Required]
    public int AllowedExceptionsCountBeforeBreaking { get; set; }

    [Required]
    public int DurationOfBreakInSeconds { get; set; }
}
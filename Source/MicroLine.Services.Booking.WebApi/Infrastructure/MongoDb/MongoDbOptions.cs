using System.ComponentModel.DataAnnotations;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb;

internal sealed class MongoDbOptions
{
    public static string SectionName => "MongoDb";

    [Required]
    public string? ConnectionString { get; set; }

    [Required]
    public string? DatabaseName { get; set; }

    [Required]
    public bool StandaloneServer { get; set; }
}
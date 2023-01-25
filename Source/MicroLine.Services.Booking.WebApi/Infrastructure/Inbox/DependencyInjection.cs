namespace MicroLine.Services.Booking.WebApi.Infrastructure.Inbox;

internal static class DependencyInjection
{
    public static IServiceCollection AddInbox(this IServiceCollection services)
    {
        services
            .AddOptions<InboxProcessorOptions>()
            .BindConfiguration(InboxProcessorOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHostedService<InboxProcessor>();

        return services;
    }

}

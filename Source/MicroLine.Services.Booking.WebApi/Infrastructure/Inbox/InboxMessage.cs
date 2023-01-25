using MicroLine.Services.Booking.Domain.Common.ValueObjects;
using MicroLine.Services.Booking.Domain.Common;
using MicroLine.Services.Booking.Domain.Common.Extensions;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.Inbox;

internal class InboxMessage : AggregateRoot
{
    public string Subject { get; init; }

    public string Content { get; init; }

    public bool Processed { get; private set; }


    private InboxMessage(Id id, string subject, string content, bool processed)
    {
        Id = id;
        Subject = subject;
        Content = content;
        Processed = processed;
    }

    public static InboxMessage Create(Id id, string subject, string content)
    {
        if (id.IsTransient)
            throw new ArgumentException($"The {nameof(Id)} of message is not valid!");

        if (subject.IsNullOrWhiteSpace())
            throw new ArgumentException($"The {nameof(Subject)} of message can not be null or empty!");

        if (content.IsNullOrWhiteSpace())
            throw new ArgumentException($"The {nameof(Content)} of message can not be null or empty!");


        return new InboxMessage(id, subject, content, false);
    }

    public void Process()
    {
        Processed = true;
    }
}

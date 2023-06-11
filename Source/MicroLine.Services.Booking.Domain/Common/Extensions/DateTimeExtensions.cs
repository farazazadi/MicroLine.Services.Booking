namespace MicroLine.Services.Booking.Domain.Common.Extensions;
public static class DateTimeExtensions
{

    public static DateTime RemoveSecondsAndSmallerTimeUnites(this DateTime dateTime)
    {
        var normalizeDateTime =
            new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind);

        return normalizeDateTime;
    }
}

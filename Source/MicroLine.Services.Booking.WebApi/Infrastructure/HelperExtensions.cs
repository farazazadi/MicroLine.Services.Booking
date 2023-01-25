using System.Text;
using System.Text.Json;

namespace MicroLine.Services.Booking.WebApi.Infrastructure;

internal static class HelperExtensions
{
    public static byte[] ToByteArray(this string value)
        => Encoding.UTF8.GetBytes(value);

    public static string ReadAsString(this ReadOnlyMemory<byte> value)
        => Encoding.UTF8.GetString(value.ToArray());

    public static string ToJsonString<TValue>(this TValue value)
        => JsonSerializer.Serialize(value);

    public static dynamic? ReadFromJson(this string jsonString, Type type)
        => JsonSerializer.Deserialize(jsonString, type);


}
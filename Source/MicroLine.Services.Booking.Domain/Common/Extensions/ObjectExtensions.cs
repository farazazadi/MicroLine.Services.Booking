namespace MicroLine.Services.Booking.Domain.Common.Extensions;

internal static class ObjectExtensions
{
    internal static Type GetRealType(this object obj)
    {
        const string efCoreProxyPrefix = "Castle.Proxies.";

        var type = obj.GetType();
        var typeString = type.ToString();

        if (type.BaseType != null && typeString.Contains(efCoreProxyPrefix))
                return type.BaseType;

        return type;
    }
}
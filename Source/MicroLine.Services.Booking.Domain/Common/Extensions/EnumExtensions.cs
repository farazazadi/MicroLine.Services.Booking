namespace MicroLine.Services.Booking.Domain.Common.Extensions;

public static class EnumExtensions
{
    public static bool IsValidEnumMember<TEnum>(this TEnum enumMember) where TEnum : struct, Enum
        => Enum.IsDefined(enumMember);

    public static bool IsValidEnumMemberOf<TEnum>(this string value, bool ignoreCase = true) where TEnum : struct, Enum
        => Enum.TryParse(value, ignoreCase, out TEnum parsedEnumMember) && IsValidEnumMember(parsedEnumMember);

    public static bool IsValidEnumMemberOf<TEnum>(this int value, bool ignoreCase = true) where TEnum : struct, Enum
        => IsValidEnumMemberOf<TEnum>(value.ToString(), ignoreCase);
}
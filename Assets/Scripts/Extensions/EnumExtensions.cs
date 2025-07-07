using System;

public static class EnumExtensions
{
    public static T ToEnum<T>(this string value, bool ignoreCase = false) where T : struct, Enum
    {
        if (Enum.TryParse(value, ignoreCase, out T result))
            return result;

        throw new ArgumentException($"'{value}' is not a valid {typeof(T)}");
    }
}

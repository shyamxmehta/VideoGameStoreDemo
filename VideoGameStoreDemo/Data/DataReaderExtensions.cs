using System.Data;
using System.Runtime.CompilerServices;

namespace AdoDotNetTutorial.Data
{
    public static class DataReaderExtensionss
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? GetNullable<T>(this IDataReader reader, int ordinal)
            where T : struct
        {
            if (reader.IsDBNull(ordinal))
                return null;

            object value = reader.GetValue(ordinal);

            // If value is already the correct type, return directly.
            if (value is T t)
                return t;

            // Convert.ChangeType handles numeric conversions like Double -> Int32, Decimal -> Double, etc.
            try
            {
                var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

                // Enum support (convert underlying numeric value to enum)
                if (targetType.IsEnum)
                {
                    var underlying = Convert.ChangeType(value, Enum.GetUnderlyingType(targetType));
                    return (T)Enum.ToObject(targetType, underlying);
                }

                var converted = Convert.ChangeType(value, targetType);
                return (T)converted;
            }
            catch
            {
                // Fallback: try direct conversion using Convert.* helpers for common numeric conversions
                try
                {
                    if (typeof(T) == typeof(int))
                        return (T)(object)Convert.ToInt32(value);
                    if (typeof(T) == typeof(long))
                        return (T)(object)Convert.ToInt64(value);
                    if (typeof(T) == typeof(double))
                        return (T)(object)Convert.ToDouble(value);
                    if (typeof(T) == typeof(float))
                        return (T)(object)Convert.ToSingle(value);
                    if (typeof(T) == typeof(decimal))
                        return (T)(object)Convert.ToDecimal(value);
                    if (typeof(T) == typeof(byte))
                        return (T)(object)Convert.ToByte(value);
                    if (typeof(T) == typeof(bool))
                        return (T)(object)Convert.ToBoolean(value);
                    if (typeof(T) == typeof(DateTime))
                        return (T)(object)Convert.ToDateTime(value);
                }
                catch
                {
                    // swallow and fall through to throw a meaningful cast exception below
                }

                throw new InvalidCastException($"Unable to convert value of type '{value.GetType()}' to '{typeof(T)}' at ordinal {ordinal}.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetStringSafe(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal)
                ? string.Empty
                : reader.GetString(ordinal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? GetNullableString(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal)
                ? null
                : reader.GetString(ordinal);
        }

        // Convenience helpers for common nullable numeric reads
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? GetNullableInt32(this IDataReader reader, int ordinal)
            => reader.GetNullable<int>(ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double? GetNullableDouble(this IDataReader reader, int ordinal)
            => reader.GetNullable<double>(ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte? GetNullableByte(this IDataReader reader, int ordinal)
            => reader.GetNullable<byte>(ordinal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime? GetNullableDateTime(this IDataReader reader, int ordinal)
            => reader.GetNullable<DateTime>(ordinal);
    }
}
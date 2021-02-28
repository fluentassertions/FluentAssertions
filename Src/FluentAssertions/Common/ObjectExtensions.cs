using System;
using System.Globalization;

namespace FluentAssertions.Common
{
    internal static class ObjectExtensions
    {
        public static bool IsSameOrEqualTo(this object actual, object expected)
        {
            if (actual is null && expected is null)
            {
                return true;
            }

            if (actual is null)
            {
                return false;
            }

            if (expected is null)
            {
                return false;
            }

            if (actual.Equals(expected))
            {
                return true;
            }

            Type expectedType = expected.GetType();
            Type actualType = actual.GetType();

            return actualType != expectedType
                && actual.IsNumericType()
                && expected.IsNumericType()
                && CanConvert(actual, expected, actualType, expectedType)
                && CanConvert(expected, actual, expectedType, actualType);
        }

        private static bool CanConvert(object source, object target, Type sourceType, Type targetType)
        {
            try
            {
                var converted = source.ConvertTo(targetType);

                return source.Equals(converted.ConvertTo(sourceType))
                     && converted.Equals(target);
            }
            catch
            {
                // ignored
                return false;
            }
        }

        private static object ConvertTo(this object source, Type targetType)
        {
            return Convert.ChangeType(source, targetType, CultureInfo.InvariantCulture);
        }

        private static bool IsNumericType(this object obj)
        {
            return obj switch
            {
                int or
                long or
                float or
                double or
                decimal or
                sbyte or
                byte or
                short or
                ushort or
                uint or
                ulong
                  => true,
                _ => false,
            };
        }
    }
}

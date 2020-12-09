using System;

namespace LACE.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNotNullOrWhiteSpace(this string input)
        {
            return input.IsNullOrWhiteSpace();
        }

        public static bool EmptyInsensitiveEquals(this string input, string other, StringComparison culture = StringComparison.Ordinal)
        {
            if (input.IsNullOrWhiteSpace() && other.IsNullOrWhiteSpace())
            {
                return true;
            }

            if (input.IsNullOrWhiteSpace() && other.IsNullOrWhiteSpace())
            {
                return false;
            }

            return input.Equals(other, culture);
        }
    }
}

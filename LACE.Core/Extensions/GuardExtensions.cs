using System;

namespace LACE.Core.Extensions
{
    public static class GuardExtensions
    {
        public static TType Guard<TType>(this TType toGuard, string name)
        {
            if (toGuard == null)
            {
                throw new ArgumentNullException(name);
            }

            return toGuard;
        }
        public static string GuardNullOrWhiteSpace(this string toGuard, string name)
        {
            if (toGuard.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(name);
            }

            return toGuard;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

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
    }
}

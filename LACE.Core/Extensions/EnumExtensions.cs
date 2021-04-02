using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACE.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string ToName<TEnum>(this TEnum named) where TEnum : struct, Enum
        {
            return Enum.GetName(named);
        }
    }
}

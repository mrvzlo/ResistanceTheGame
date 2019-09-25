using System;
using System.Linq;
using Resistance.Helpers;

namespace Resistance
{
    public static class StringHelper
    {
        public static bool EqualsAny(string baseStr, params string[] values)
        {
            return values.Any(x => x.Equals(baseStr, StringComparison.OrdinalIgnoreCase))
                || values.Any(x => $"{x}@{Rules.BotName}".Equals(baseStr, StringComparison.OrdinalIgnoreCase));
        }

        public static bool BeginsWithAny(string baseStr, params string[] values)
        {
            return values.Any(x => baseStr.StartsWith(x, StringComparison.OrdinalIgnoreCase));
        }
    }
}

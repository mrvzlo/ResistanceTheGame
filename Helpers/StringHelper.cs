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
    }
}

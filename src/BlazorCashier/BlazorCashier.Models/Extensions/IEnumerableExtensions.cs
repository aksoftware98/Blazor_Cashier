using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorCashier.Models.Extensions
{
    public static class IEnumerableExtensions
    {
        public static string AllInOne(this IEnumerable<string> values)
            => values.Aggregate((e1, e2) => $"{e1}{Environment.NewLine}{e2}");
    }
}

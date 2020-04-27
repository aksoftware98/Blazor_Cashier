using System;

namespace BlazorCashier.Models.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime NormalizedDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
    }
}

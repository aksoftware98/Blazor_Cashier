using BlazorCashier.Models.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace BlazorCashier.Server.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static string ErrorsAllInOne(this ModelStateDictionary modelState)
            => modelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).AllInOne();
    }
}

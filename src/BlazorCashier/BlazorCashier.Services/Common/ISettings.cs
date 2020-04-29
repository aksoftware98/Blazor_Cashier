using System.Collections.Generic;

namespace BlazorCashier.Services.Common
{
    public interface ISettings
    {
        public IDictionary<int, decimal> PointsValues { get; }
        public IEnumerable<string> SupportedExtensions { get; }
        public bool RestrictPaymentOnSession { get; }
        public int MaxImageSize { get; }
    }
}

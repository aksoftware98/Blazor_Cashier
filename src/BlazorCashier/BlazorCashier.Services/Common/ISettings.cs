using System.Collections.Generic;

namespace BlazorCashier.Services.Common
{
    public interface ISettings
    {
        public IEnumerable<string> SupportedExtensions { get; }
        public int MaxImageSize { get; }
    }
}

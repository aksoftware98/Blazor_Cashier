using System.Collections.Generic;

namespace BlazorCashier.Services.Common
{
    public class Settings : ISettings
    {
        #region Private Members

        #endregion

        #region Public Properties

        public IEnumerable<string> SupportedExtensions { get; }
        public int MaxImageSize { get; }

        #endregion

        #region Constructors

        public Settings()
        {
            SupportedExtensions = new List<string>() { ".jpeg", ".jpg", ".png", ".bmp" };
            MaxImageSize = 2 * 1000 * 1000; // 2 MB
        }

        #endregion
    }
}

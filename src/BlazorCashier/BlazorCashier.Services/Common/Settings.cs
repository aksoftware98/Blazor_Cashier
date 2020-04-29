using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using System.Net;

namespace BlazorCashier.Services.Common
{
    public class Settings : ISettings
    {
        #region Private Members

        #endregion

        #region Public Properties

        public IDictionary<int, decimal> PointsValues { get; }
        public IEnumerable<string> SupportedExtensions { get; }
        public bool RestrictPaymentOnSession { get; }
        public int MaxImageSize { get; }

        #endregion

        #region Constructors

        public Settings()
        {
            SupportedExtensions = new List<string>() { ".jpeg", ".jpg", ".png", ".bmp" };
            MaxImageSize = 2 * 1000 * 1000; // 2 MB
            RestrictPaymentOnSession = false;
            PointsValues = new Dictionary<int, decimal>
            {
                { 200000, 20 },
                { 300000, 35 },
                { 500000, 60 },
            };
        }

        #endregion
    }
}

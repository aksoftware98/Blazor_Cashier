using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCashier.Shared.Domain
{
    public class CurrencyDetail
    {
        public string Id { get; set; }
        public string Code { get; set; }
    }

    public class CountryDetail
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}

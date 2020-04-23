using BlazorCashier.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorCashier.Shared.Domain
{
    public class DiscountDetail
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Value { get; set; }
        public ICollection<DiscountItemDetail> DiscountItems { get; set; }

        [JsonIgnore]
        public string OrganizationId { get; set; }

        public DiscountDetail()
        {

        }

        public DiscountDetail(Discount discount)
        {
            Id = discount.Id;
            Description = discount.Description;
            StartDate = discount.StartDate;
            EndDate = discount.EndDate;
            Value = discount.Value;
            DiscountItems =
                new List<DiscountItemDetail>(discount.DiscountItems.Select(i => new DiscountItemDetail(i)));
        }
    }
}

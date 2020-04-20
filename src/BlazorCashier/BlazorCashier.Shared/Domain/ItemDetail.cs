using BlazorCashier.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorCashier.Shared.Domain
{
    public class ItemDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal SellingPrice { get; set; }
        public string Barcode { get; set; }
        public string CountryOfOrigin { get; set; }
        public int Points { get; set; }
        public string AdditionalProperties { get; set; }
        public string OrganizationId { get; set; }

        public ItemDetail()
        {

        }

        public ItemDetail(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            SellingPrice = item.SellingPrice;
            Barcode = item.Barcode;
            CountryOfOrigin = item.CountryOfOrigin;
            Points = item.Points;
            AdditionalProperties = item.AdditionalProperties;
        }
    }
}

using BlazorCashier.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlazorCashier.Shared.Domain
{
    public class BillDetail
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public decimal Total { get; set; }
        public string Note { get; set; }
        public VendorDetail Vendor { get; set; }
        public ICollection<BillItemDetail> BillItems { get; set; }
        public string organizationId { get; set; }

        public BillDetail()
        {

        }

        public BillDetail(Bill bill)
        {
            Id = bill.Id;
            Number = bill.Number;
            Total = bill.Total;
            Note = bill.Note;
            Vendor = new VendorDetail
            {
                Id = bill.Vendor.Id,
                FirstName = bill.Vendor.FirstName,
                LastName = bill.Vendor.LastName
            };
            BillItems = new List<BillItemDetail>
                (bill.BillItems.Select(item => new BillItemDetail(item)));
        }
    }
}

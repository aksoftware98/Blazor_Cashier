using System.Collections.Generic;

namespace BlazorCashier.Models
{
    /// <summary>
    /// Represents the data of a bill
    /// </summary>
    public class Bill : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Bill() 
        {
            BillItems = new List<BillItem>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The number of the bill
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// The total bill value
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// The note related to the bill
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// The identifier of the related vendor
        /// </summary>
        public string VendorId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related vendor
        /// </summary>
        public virtual Vendor Vendor { get; set; }

        /// <summary>
        /// The related bill items
        /// </summary>
        public virtual ICollection<BillItem> BillItems { get; set; }

        #endregion
    }
}

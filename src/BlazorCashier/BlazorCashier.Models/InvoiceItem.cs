using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCashier.Models
{
    /// <summary>
    /// Represents the data for an invoice item
    /// </summary>
    public class InvoiceItem : OrganizationRelatedEntity
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvoiceItem() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The description of the invoice
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The price of the invoice
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// The final price of the invoice
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal FinalPrice { get; set; }

        /// <summary>
        /// The discount applied to the <see cref="Stock"/> related if there is any
        /// </summary>
        public float Discount { get; set; }

        /// <summary>
        /// The identifier of the related invoice
        /// </summary>
        public string InvoiceId { get; set; }

        /// <summary>
        /// The identifier of the related stock
        /// </summary>
        public string StockId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related invoice
        /// </summary>
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// The related stock
        /// </summary>
        public virtual Stock Stock { get; set; }

        #endregion
    }
}

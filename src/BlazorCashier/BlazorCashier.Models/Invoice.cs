using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCashier.Models
{
    /// <summary>
    /// Represents the data of an invoice
    /// </summary>
    public class Invoice : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Invoice() 
        {
            InvoiceItems = new List<InvoiceItem>(); 
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The invoice number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// The invoice original price
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// The invoice final price
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal FinalPrice { get; set; }

        /// <summary>
        /// The discount applied to the invoice if there is any discount
        /// </summary>
        public float Discount { get; set; }

        /// <summary>
        /// Indicates whether the invoice is paid by customer's points or not
        /// </summary>
        public int PaidWithPoints { get; set; }

        /// <summary>
        /// Points used to pay the invoice with
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Invoice note
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// The identifier of related customer
        /// </summary>
        public string CustomerId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related customer
        /// </summary>
        public virtual Customer Customer { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        #endregion
    }
}

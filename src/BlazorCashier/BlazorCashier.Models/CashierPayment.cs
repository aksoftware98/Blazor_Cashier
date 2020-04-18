using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCashier.Models
{
    /// <summary>
    /// Represents the data of a cashier payment
    /// </summary>
    public class CashierPayment : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CashierPayment() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The cash entered to the cashier
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal CashEntered { get; set; }

        /// <summary>
        /// The cash change of the entered cash for the payment
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Change { get; set; }

        /// <summary>
        /// The identifier of the related session
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// The identifier of the related invoice
        /// </summary>
        public string InvoiceId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related invoice
        /// </summary>
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// The related session
        /// </summary>
        public virtual Session Session { get; set; }

        #endregion
    }
}

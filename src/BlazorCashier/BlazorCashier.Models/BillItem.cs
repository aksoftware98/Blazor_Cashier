using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCashier.Models
{
    /// <summary>
    /// Represents the data of a bill item
    /// </summary>
    public class BillItem : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BillItem() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The description of the bill item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The quantity of the bill item
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price of the bill item
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// The identifier of the related stock
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// The identifier of the related bill
        /// </summary>
        public string BillId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related bill
        /// </summary>
        public virtual Bill Bill { get; set; }

        /// <summary>
        /// The related stock
        /// </summary>
        public virtual Stock Stock { get; set; }

        #endregion
    }
}

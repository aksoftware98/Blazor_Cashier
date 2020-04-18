namespace BlazorCashier.Models
{
    /// <summary>
    /// Represents the data of a discount item
    /// </summary>
    public class DiscountItem : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountItem() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The identifier of the related stock
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// The identifier of the related discount
        /// </summary>
        public string DiscountId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related discount
        /// </summary>
        public virtual Discount Discount { get; set; }

        /// <summary>
        /// The related stock
        /// </summary>
        public virtual Stock Stock { get; set; }

        #endregion
    }
}

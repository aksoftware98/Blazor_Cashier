namespace BlazorCashier.Shared.DomainModels
{
    /// <summary>
    /// Represents the data of an item
    /// </summary>
    public class Item : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Item() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The buying price of the item for the owned organization
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The selling price of the item
        /// </summary>
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// The barcode of the item
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// The country of origin of the item
        /// </summary>
        public string CountryOfOrigin { get; set; }

        /// <summary>
        /// The amount of points that an item gives when purchased
        /// </summary>
        public double Points { get; set; }

        /// <summary>
        /// Additional user-created properties saved as JSON
        /// </summary>
        public string AdditionalProperties { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}

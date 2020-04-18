namespace BlazorCashier.Shared.DomainModels
{
    /// <summary>
    /// Represents the data of a vendor
    /// </summary>
    public class Vendor : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Vendor() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The first name of the vendor
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the vendor
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The first address of the vendor
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// The second address of the vendor
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// The phone of the vendor
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The telephone of the vendor
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// The email of the vendor
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The website of the vendor
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// The city of the vendor
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The country of the vendor
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Notes for the vendor
        /// </summary>
        public string Note { get; set; }

        #endregion
    }
}

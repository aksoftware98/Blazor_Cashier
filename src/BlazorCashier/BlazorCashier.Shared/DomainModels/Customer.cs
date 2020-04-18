using System.Collections.Generic;

namespace BlazorCashier.Shared.DomainModels
{
    /// <summary>
    /// Represents the data of a customer
    /// </summary>
    public class Customer : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Customer() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The first name of the customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The phone of the customer
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The email of the customer
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The address of the customer
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The street address of the customer
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// The city of the customer
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The points of the customer
        /// defaulted to 0
        /// </summary>
        public double Points { get; set; } = 0;

        /// <summary>
        /// The barcode of the customer 
        /// </summary>
        public string Barcode { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related invoices
        /// </summary>
        public virtual ICollection<Invoice> Invoices { get; set; }

        #endregion
    }
}

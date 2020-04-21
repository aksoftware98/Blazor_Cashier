using System.Collections.Generic;

namespace BlazorCashier.Models
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
        public Customer() 
        {
            Invoices = new List<Invoice>(); 
        }

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
        /// The related country identifier
        /// </summary>
        public string CountryId { get; set; }

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
        public int Points { get; set; } = 0;

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

        public virtual Country Country { get; set; }
        #endregion
    }
}

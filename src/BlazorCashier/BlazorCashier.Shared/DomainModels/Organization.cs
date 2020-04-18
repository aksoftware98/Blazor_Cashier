using System;
using System.Collections.Generic;

namespace BlazorCashier.Shared.DomainModels
{
    /// <summary>
    /// Represents the data for an organization
    /// </summary>
    public class Organization : BaseEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Organization() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the organization
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The address of an organization
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The city of the organization
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The country of the organization
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The phone of the organization
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The telephone of the organization
        /// </summary>
        public string TelePhone { get; set; }

        /// <summary>
        /// The email of the organization
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The organization owner name
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// The registration date of the organization
        /// </summary>
        public DateTimeOffset RegistrationDate { get; set; }

        /// <summary>
        /// The main website of the organization
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// The global financial number of the organization
        /// </summary>
        public string FinancialNumber { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related system users
        /// </summary>
        public virtual ICollection<SystemUser> SystemUsers { get; set; }

        #endregion
    }
}

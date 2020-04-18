using System;

namespace BlazorCashier.Shared.DomainModels
{
    /// <summary>
    /// Represents the data of a system user
    /// </summary>
    public class SystemUser : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SystemUser() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The birth date of the user
        /// </summary>
        public DateTimeOffset Birthdate { get; set; }

        /// <summary>
        /// Indicates whether the user is blocked or not
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// The address of the user
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The city of the user
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The country of the user
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The Profile picture file name of the user
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// The identifier of the related organization
        /// </summary>
        public string OrganizationId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related organization
        /// </summary>
        public virtual Organization Organization { get; set; }

        #endregion
    }
}

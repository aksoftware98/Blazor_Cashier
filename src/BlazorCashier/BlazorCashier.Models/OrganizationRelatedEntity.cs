using BlazorCashier.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCashier.Models
{
    /// <summary>
    /// Represents the base class of domain models that are related to an organization
    /// </summary>
    public abstract class OrganizationRelatedEntity : BaseEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrganizationRelatedEntity() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The organization identifier
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// The system user identifier who created the current object
        /// </summary>
        [ForeignKey(nameof(CreatedUser))]
        public string CreatedById { get; set; }

        /// <summary>
        /// The system user identifier who modified the current object
        /// </summary>
        [ForeignKey(nameof(ModifiedUser))]
        public string ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related organization
        /// </summary>
        public virtual Organization Organization { get; set; }

        public virtual ApplicationUser CreatedUser { get; set; }
        public virtual ApplicationUser ModifiedUser { get; set; }

        #endregion
    }
}

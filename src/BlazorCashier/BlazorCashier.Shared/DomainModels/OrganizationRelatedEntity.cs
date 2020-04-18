namespace BlazorCashier.Shared.DomainModels
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
        public string CreatedById { get; set; }

        /// <summary>
        /// The system user identifier who modified the current object
        /// </summary>
        public string ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related organization
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// The related creator system user
        /// </summary>
        public virtual SystemUser CreatorUser { get; set; }

        /// <summary>
        /// The related modifier system user
        /// </summary>
        public virtual SystemUser ModifierUser { get; set; }

        #endregion
    }
}

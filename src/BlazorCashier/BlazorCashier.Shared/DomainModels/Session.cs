using System;
using System.Collections;
using System.Collections.Generic;

namespace BlazorCashier.Shared.DomainModels
{
    /// <summary>
    /// Represents the data of a session
    /// </summary>
    public class Session : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Session() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the session
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the session
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The start date of the session
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// The end date of the session
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// The original total of the session
        /// </summary>
        public decimal OriginalTotal { get; set; }

        /// <summary>
        /// The final total of the session
        /// </summary>
        public decimal FinalTotal { get; set; }

        /// <summary>
        /// The profit total of the session
        /// </summary>
        public decimal ProfitTotal { get; set; }

        /// <summary>
        /// The identifier of the user related to the current session
        /// </summary>
        public string UserId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related user to the current session
        /// </summary>
        public virtual SystemUser SystemUser { get; set; }

        /// <summary>
        /// The related cashier payments
        /// </summary>
        public virtual ICollection<CashierPayment> CashierPayments { get; set; }

        #endregion
    }
}

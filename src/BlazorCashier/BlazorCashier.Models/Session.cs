using BlazorCashier.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCashier.Models
{
    /// <summary>
    /// Represents the data of a session
    /// </summary>
    public class Session : OrganizationRelatedEntity, IComparable<Session>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Session() 
        {
            CashierPayments = new List<CashierPayment>();
        }

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
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date of the session
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The original total of the session
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal OriginalTotal { get; set; }

        /// <summary>
        /// The final total of the session
        /// </summary>
        [Column(TypeName ="decimal(8, 2)")]
        public decimal FinalTotal { get; set; }

        /// <summary>
        /// The profit total of the session
        /// </summary>
        [Column(TypeName ="decimal(8, 2)")]
        public decimal ProfitTotal { get; set; }

        /// <summary>
        /// The identifier of the user related to the current session
        /// </summary>
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related user to the current session
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// The related cashier payments
        /// </summary>
        public virtual ICollection<CashierPayment> CashierPayments { get; set; }

        #endregion

        #region Public Methods

        public int CompareTo(Session session)
        {
            return Id.CompareTo(session.Id);
        }

        #endregion
    }
}

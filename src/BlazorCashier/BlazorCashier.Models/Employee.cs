using BlazorCashier.Models.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCashier.Models
{
    public class Employee : OrganizationRelatedEntity
    {
        #region Constructors

        public Employee() { }

        #endregion

        #region Public Properties

        public string City { get; set; }
        public string StreetAddress { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ApplicationUser User { get; set; }

        #endregion
    }
}

using BlazorCashier.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorCashier.Models
{
    public class Employee : OrganizationRelatedEntity
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

       public virtual ApplicationUser User { get; set; }

        public string UserId { get; set; }
 
    }
}

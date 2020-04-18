using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCashier.Models
{
    public class Currency : BaseEntity
    {

        public Currency()
        {
            Organizations = new List<Organization>(); 
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;

namespace BlazorCashier.Shared.DomainModels
{
    /// <summary>
    /// Represents the data of a discount
    /// </summary>
    public class Discount : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Discount() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The description of the discount
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The start date of the discount
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// The end date of the discount
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// The value of the discount
        /// </summary>
        public int Value { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related discount items
        /// </summary>
        public virtual ICollection<DiscountItem> DiscountItems { get; set; }

        #endregion
    }
}
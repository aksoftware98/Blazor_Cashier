﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCashier.Models
{
    /// <summary>
    /// Represents the data of a stock
    /// </summary>
    public class Stock : OrganizationRelatedEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Stock() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// The quantity of the stock
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price of the stock
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// The selling price of the stock
        /// </summary>
        [Column(TypeName = "decimal(8, 2)")]
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// The points given when stock is purchased
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// The identifier of the related item
        /// </summary>
        public string ItemId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// The related item
        /// </summary>
        public virtual Item Item { get; set; }

        #endregion
    }
}

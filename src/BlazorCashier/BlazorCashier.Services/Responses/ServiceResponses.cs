using BlazorCashier.Models;
using System.Collections.Generic;

namespace BlazorCashier.Services.Responses
{
    /// <summary>
    /// Represents a service response
    /// </summary>
    public class ServiceResponse
    {
        #region Public Properties

        /// <summary>
        /// Indicates whether the operation was successful or not
        /// </summary>
        public bool IsSuccess => string.IsNullOrEmpty(Error);

        /// <summary>
        /// The error returned by the service if there is any
        /// </summary>
        public string Error { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServiceResponse(string error = null)
            => Error = error;

        #endregion
    }

    /// <summary>
    /// Represents a response which has a single entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public class SingleEntityResponse<TEntity> : ServiceResponse where TEntity : BaseEntity
    {
        #region Public Properties

        /// <summary>
        /// The entity returned by the service
        /// </summary>
        public TEntity Entity { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SingleEntityResponse(
            TEntity entity = null,
            string error = null) : base(error)
            => Entity = entity;

        #endregion
    }

    /// <summary>
    /// Represents a response which has a collection of entities
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public class CollectionEntityResponse<TEntity> : ServiceResponse where TEntity : BaseEntity
    {
        #region Public Properties

        /// <summary>
        /// Entities returned by the service
        /// </summary>
        public IEnumerable<TEntity> Entities { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CollectionEntityResponse(
            IEnumerable<TEntity> entities = null,
            string error = null) : base(error)
            => Entities = entities;

        #endregion
    }
}

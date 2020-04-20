using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Models.Data
{
    /// <summary>
    /// Represents a repository for a <see cref="TEntity"/> entity
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Private Members

        private readonly IDbContext _dbContext;
        private DbSet<TEntity> _entities;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Repository(IDbContext dbContext) => _dbContext = dbContext;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public IQueryable<TEntity> Table => Entities;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature)
        /// Used for operations that involve read-only
        /// Good for search optimization
        /// </summary>
        public IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected DbSet<TEntity> Entities 
            => _entities ?? (_entities = _dbContext.Set<TEntity>());

        #endregion

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await Entities.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //TODO: The e.Message should be logged 

                // Pop up the exception to the caller
                throw exception;
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                await Entities.AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //TODO: The e.Message should be logged 

                // Pop up the exception to the caller
                throw exception;
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //TODO: The e.Message should be logged 

                // Pop up the exception to the caller
                throw exception;
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.UpdateRange(entities);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //TODO: The e.Message should be logged 

                // Pop up the exception to the caller
                throw exception;
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task AttachAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Attach(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //TODO: The e.Message should be logged 

                // Pop up the exception to the caller
                throw exception;
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task AttachAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.AttachRange(entities);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //TODO: The e.Message should be logged 

                // Pop up the exception to the caller
                throw exception;
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //TODO: The e.Message should be logged 

                // Pop up the exception to the caller
                throw exception;
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.RemoveRange(entities);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //TODO: The e.Message should be logged 

                // Pop up the exception to the caller
                throw exception;
            }
        }

        #endregion
    }
}

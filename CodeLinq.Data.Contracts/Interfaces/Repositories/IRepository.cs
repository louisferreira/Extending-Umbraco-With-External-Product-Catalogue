using CodeLinq.Data.Contracts.Interfaces.Entities.Base;
using CodeLinq.Data.Contracts.Interfaces.Infrastructure;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CodeLinq.Data.Contracts.Interfaces.Repositories
{
    /// <summary>
    /// A Generic interface for a repository that performs operations on a datastore based on a specified Entity type.
    /// </summary>
    /// <typeparam name="TEntity">The Entity type that this repository is bound to.</typeparam>
    public interface IRepository<TEntity> where TEntity : class, IUniqueIdentifier
    {
        /// <summary>
        /// Method to get a single Entity based on it's unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier for the entity to retrieve</param>
        /// <returns>An instance of the specified type</returns>
        TEntity Get(object Id);

        /// <summary>
        /// Method to get a collection of Entities based on an expression
        /// </summary>
        /// <param name="filter">The Linq Expression to filter the collection on.</param>
        /// <returns>An IQueryable collection of Entities.</returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Method to get a collection of all Entities from the datastore.
        /// </summary>
        /// <returns>An IQueryable collection of Entities.</returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// Method to insert a new Entity into the datastore. The repository is responsible for populating the unique identifier field of the entity before or after insertion to the datastore.
        /// </summary>
        /// <param name="entity">The entity instance to insert</param>
        /// <returns>An IOperationResult instance containing information about the operation</returns>
        IOperationResult<TEntity> Insert(TEntity entity);

        /// <summary>
        /// Method to delete an Entity from the datastore.
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity instance to delete</param>
        /// <returns>An IOperationResult instance containing information about the operation</returns>
        IOperationResult<TEntity> Delete(object entityId);

        /// <summary>
        /// Method to delete an Entity from the datastore.
        /// </summary>
        /// <param name="entity">The instance of the entity to delete</param>
        /// <returns>An IOperationResult instance containing information about the operation</returns>
        IOperationResult<TEntity> Delete(TEntity entity);

        /// <summary>
        /// Method to update an Entity to the datastore.
        /// </summary>
        /// <param name="entity">The entity instance to update</param>
        /// <returns>An IOperationResult instance containing information about the operation</returns>
        IOperationResult<TEntity> Update(TEntity entity);
    }
}

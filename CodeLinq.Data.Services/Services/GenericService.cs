using CodeLinq.Data.Contracts.Interfaces.Entities.Base;
using CodeLinq.Data.Contracts.Interfaces.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Repositories;
using CodeLinq.Data.Contracts.Interfaces.Services;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CodeLinq.Data.Services.Services
{
    /// <summary>
    /// A Generic Service that performs operations on one or more IRepositories for a specified Entity type.
    /// </summary>
    /// <typeparam name="TEntity">The Entity type that this Service is bound to.</typeparam>
    public class GenericService<TEntity> : IService<TEntity> where TEntity : class, IUniqueIdentifier
    {
        private readonly IRepository<TEntity> repository;

        public GenericService(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Method to get a single Entity based on it's unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier for the entity to retrieve</param>
        /// <returns>An instance of the specified type</returns>
        public TEntity Get(object Id)
        {
            return repository.Get(Id);
        }

        /// <summary>
        /// Method to get a collection of Entities based on an expression
        /// </summary>
        /// <param name="filter">The Linq Expression to filter the collection on.</param>
        /// <returns>An IQueryable collection of Entities.</returns>
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return repository.Get(filter);
        }

        /// <summary>
        /// Method to get a collection of all Entities from the datastore.
        /// </summary>
        /// <returns>An IQueryable collection of Entities.</returns>
        public IQueryable<TEntity> Get()
        {
            return repository.Get();
        }

        /// <summary>
        /// Method to delete an Entity from the datastore.
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity instance to delete</param>
        /// <returns>An IOperationResult instance containing information about the operation</returns>
        public IOperationResult<TEntity> Delete(object entityId)
        {
            return repository.Delete(entityId);
        }

        /// <summary>
        /// Method to delete an Entity from the datastore.
        /// </summary>
        /// <param name="entity">The instance of the entity to delete</param>
        /// <returns>An IOperationResult instance containing information about the operation</returns>
        public IOperationResult<TEntity> Delete(TEntity entity)
        {
            return repository.Delete(entity);
        }

        /// <summary>
        /// Method to insert a new Entity into the datastore. The repository is responsible for populating the unique identifier field of the entity before or after insertion to the datastore.
        /// </summary>
        /// <param name="entity">The entity instance to insert</param>
        /// <returns>An IOperationResult instance containing information about the operation</returns>
        public IOperationResult<TEntity> Insert(TEntity entity)
        {
            return repository.Insert(entity);
        }

        /// <summary>
        /// Method to update an Entity to the datastore.
        /// </summary>
        /// <param name="entity">The entity instance to update</param>
        /// <returns>An IOperationResult instance containing information about the operation</returns>
        public IOperationResult<TEntity> Update(TEntity entity)
        {
            return repository.Update(entity);
        }
    }
}

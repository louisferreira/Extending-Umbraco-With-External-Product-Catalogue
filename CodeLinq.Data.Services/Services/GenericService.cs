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


        public IOperationResult<TEntity> Delete(object entityId)
        {
            return repository.Delete(entityId);
        }

        public IOperationResult<TEntity> Delete(TEntity entity)
        {
            return repository.Delete(entity);
        }

        public TEntity Get(object Id)
        {
            return repository.Get(Id);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return repository.Get(filter);
        }

        public IQueryable<TEntity> Get()
        {
            return repository.Get();
        }

        public IOperationResult<TEntity> Insert(TEntity entity)
        {
            return repository.Insert(entity);
        }

        public IOperationResult<TEntity> Update(TEntity entity)
        {
            return repository.Update(entity);
        }
    }
}

using CodeLinq.Data.Contracts.Interfaces.Entities;
using System.Collections.Generic;

namespace CodeLinq.Data.Contracts.Interfaces.Services
{
    /// <summary>
    /// A Strongly typed interface for a Service that performs operations for ICategory types.
    /// </summary>
    public interface ICategoryService : IService<ICategory>
    {
        /// <summary>
        /// A method that returns a collection of IProduct by a CategoryId
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category</param>
        /// <returns>An IEnumerable<IProduct> filtered by category</returns>
        IEnumerable<IProduct> GetProductsByCategoryId(object categoryId);

        /// <summary>
        /// A method to get all root Categories
        /// </summary>
        /// <returns>An IEnumerable<ICategory> filtered by root elements</returns>
        IEnumerable<ICategory> GetRootCategories();

        /// <summary>
        /// A method to return all Categories by a specified parent id.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the parent category</param>
        /// <returns>An IEnumerable<ICategory> filtered by a parent element</returns>
        IEnumerable<ICategory> GetCategoriesByParentId(object categoryId);
    }
}

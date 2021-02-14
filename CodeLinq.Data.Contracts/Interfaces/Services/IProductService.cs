using CodeLinq.Data.Contracts.Interfaces.Entities;
using CodeLinq.Data.Contracts.Interfaces.Infrastruture;
using System.Collections.Generic;

namespace CodeLinq.Data.Contracts.Interfaces.Services
{
    /// <summary>
    /// A Strongly typed interface for a Service that performs operations for IProduct types.
    /// </summary>
    public interface IProductService : IService<IProduct>
    {
        /// <summary>
        /// A method that returns a collection of IProduct by a CategoryId
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category</param>
        /// <returns>An IEnumerable<IProduct> filtered by category</returns>
        IEnumerable<IProduct> GetByCategoryId(object categoryId);

        /// <summary>
        /// A method that returns a collection of IProduct variants
        /// </summary>
        /// <param name="productId">The unique identifier of the base product</param>
        /// <returns>An IEnumerable<IProduct> </returns>
        IEnumerable<IProduct> GetVariants(object productId);

        /// <summary>
        /// A method that returns a collection of IMedia belonging to a product
        /// </summary>
        /// <param name="productId">The unique identifier of the product</param>
        /// <returns>An IEnumerable<IMedia> entities</returns>
        IEnumerable<IMedia> GetMedia(object productId);

        /// <summary>
        /// A method that returns a collection of IMedia belonging to a product, filtered by media type
        /// </summary>
        /// <param name="productId">The unique identifier of the product</param>
        /// <param name="mediaType">The type of media to get</param>
        /// <returns>An IEnumerable<IMedia> entities</returns>
        IEnumerable<IMedia> GetMedia(object productId, MediaType mediaType);

    }
}

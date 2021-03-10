using CodeLinq.Data.Contracts.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Entities;
using CodeLinq.Data.Contracts.Interfaces.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Repositories;
using CodeLinq.Data.Contracts.Interfaces.Services;
using CodeLinq.Data.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeLinq.Data.Services.Services
{
    /// <summary>
    /// A Service that performs operations for IProduct types.
    /// </summary>
    public class ProductService : GenericService<IProduct>, IProductService
    {
        private readonly IRepository<IProduct> productRepository;
        private readonly IRepository<ICategoryProduct> categoryProductRepository;
        private readonly IMediaService mediaService;

        public ProductService(IRepository<IProduct> productRepository, 
            IRepository<ICategoryProduct> categoryProductRepository, 
            IMediaService mediaService) : base(productRepository)
        {
            this.productRepository = productRepository;
            this.categoryProductRepository = categoryProductRepository;
            this.mediaService = mediaService;
        }

        /// <summary>
        /// A method that returns a collection of IProduct by a CategoryId
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category</param>
        /// <returns>An IEnumerable<IProduct> filtered by category</returns>
        public IEnumerable<IProduct> GetByCategoryId(object categoryId)
        {
            // get all categoryProduct entities for this catId
            var related = categoryProductRepository.Get(x => x.CategoryId.Equals(categoryId));

            // create an array of productId
            var prodIds = related.Select(x => x.ProductId).ToArray();

            //return all categories where the id is in the array
            return productRepository.Get(x => prodIds.Contains(x.Id));
        }

        /// <summary>
        /// A method that returns a collection of IMedia belonging to a product
        /// </summary>
        /// <param name="productId">The unique identifier of the product</param>
        /// <returns>An IEnumerable<IMedia> entities</returns>
        public IEnumerable<IMedia> GetMedia(object productId)
        {
            return mediaService.Get(productId, EntityType.Product);
        }

        /// <summary>
        /// A method that returns a collection of IMedia belonging to a product, filtered by media type
        /// </summary>
        /// <param name="productId">The unique identifier of the product</param>
        /// <param name="mediaType">The type of media to get</param>
        /// <returns>An IEnumerable<IMedia> entities</returns>
        public IEnumerable<IMedia> GetMedia(object productId, MediaType mediaType)
        {
            return mediaService.Get(productId, EntityType.Product, mediaType);
        }

        /// <summary>
        /// A method that returns a collection of IProduct variants
        /// </summary>
        /// <param name="productId">The unique identifier of the base product</param>
        /// <returns>An IEnumerable<IProduct> </returns>
        public IEnumerable<IProduct> GetVariants(object productId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a Product from the data store, and dependancies (CategoryProduct and IMedia)
        /// </summary>
        /// <param name="entityId">The unique identifier of the product to delete.</param>
        /// <returns>An IOperationResult instance containing information abou the operation</returns>
        public new IOperationResult<IProduct> Delete(object entityId)
        {
            try
            {
                // first delete all media
                var allMedia = mediaService.Get(entityId, EntityType.Product).ToList();
                foreach (var media in allMedia)
                {
                    var result = mediaService.Delete(media);
                    if (result.OperationOutcome == OperationOutcome.Success)
                        continue;

                    // Log warning to say media not found... ignoring
                    // Take steps to mark this media as unresolveable / flagged for deletion etc
                }

                // delete all category products
                var allCatProd = categoryProductRepository.Get(x => x.ProductId.Equals(entityId)).ToList();
                foreach (var item in allCatProd)
                {
                    var result = categoryProductRepository.Delete(item.Id);
                    if (result.OperationOutcome == OperationOutcome.Success)
                        continue;

                    // Log warning to say categoryProduct not found... ignoring

                }

                // then delete product
                return productRepository.Delete(entityId);
            }
            catch (Exception ex)
            {
                return new OperationResult<IProduct>
                {
                    Entity = null,
                    Message = ex.Message,
                    ResultCode = ex.HResult,
                    OperationOutcome = OperationOutcome.InternalError
                };
            }
        }

        /// <summary>
        /// Deletes a Product from the data store, and dependancies (CategoryProduct and IMedia)
        /// </summary>
        /// <param name="entity">The product to delete.</param>
        /// <returns>An IOperationResult instance containing information abou the operation</returns>
        public new IOperationResult<IProduct> Delete(IProduct entity)
        {
            return this.Delete(entity.Id);
        }

    }
}

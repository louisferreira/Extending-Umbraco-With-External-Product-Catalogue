using CodeLinq.Data.Contracts.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Entities;
using CodeLinq.Data.Contracts.Interfaces.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Repositories;
using CodeLinq.Data.Contracts.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;

namespace CodeLinq.Data.Services.Services
{
    public class CategoryService : GenericService<ICategory>, ICategoryService
    {
        private readonly IRepository<ICategory> categoryRepository;
        private readonly IRepository<IProduct> productRepository;
        private readonly IRepository<ICategoryProduct> categoryProductRepository;
        private readonly IMediaService mediaService;

        public CategoryService(IRepository<ICategory> categoryRepository, IRepository<IProduct> productRepository, IRepository<ICategoryProduct> categoryProductRepository, IMediaService mediaService) : base(categoryRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
            this.categoryProductRepository = categoryProductRepository;
            this.mediaService = mediaService;
        }

        public IEnumerable<ICategory> GetCategoriesByParentId(object categoryId)
        {
            return categoryRepository.Get(x => x.ParentCategoryId != null && x.ParentCategoryId.Equals(categoryId));
        }

        public IEnumerable<IProduct> GetProductsByCategoryId(object categoryId)
        {
            // get all CategoryProduct entities for this categoryId
            var related = categoryProductRepository
                .Get(x => x.CategoryId.Equals(categoryId))
                .ToList();

            // if none exists, return an empty enumerable object
            if (related == null || !related.Any())
            {
                return Enumerable.Empty<IProduct>();
            }

            // transform the list of CategoryProducts into an array of ProductIds
            var productIdArray = related
                .Select(x => x.ProductId)
                .ToArray();

            // now get all products where the ProductId is in that array
            var products = productRepository
                .Get(x => productIdArray.Contains(x.Id))
                .ToList();

            return products;
        }

        public IEnumerable<ICategory> GetRootCategories()
        {
            return categoryRepository.Get(x => x.ParentCategoryId == null);
        }
        
        public IEnumerable<IMedia> GetMedia(object categoryId)
        {
            return mediaService.Get(categoryId, EntityType.Category);
        }

        public IEnumerable<IMedia> GetMedia(object categoryId, MediaType mediaType)
        {
            return mediaService.Get(categoryId, EntityType.Category, mediaType);
        }
    }
}

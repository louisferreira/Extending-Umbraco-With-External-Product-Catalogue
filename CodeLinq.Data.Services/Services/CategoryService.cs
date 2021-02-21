using CodeLinq.Data.Contracts.Interfaces.Entities;
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

        public CategoryService(IRepository<ICategory> categoryRepository, IRepository<IProduct> productRepository, IRepository<ICategoryProduct> categoryProductRepository) : base(categoryRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
            this.categoryProductRepository = categoryProductRepository;
        }

        public IEnumerable<ICategory> GetCategoriesByParentId(object categoryId)
        {
            return categoryRepository.Get(x => x.ParentCategoryId != null && x.ParentCategoryId.ToString() == categoryId.ToString());
        }

        public IEnumerable<IProduct> GetProductsByCategoryId(object categoryId)
        {
            // get all CategoryProduct entities for this categoryId
            var related = categoryProductRepository
                .Get(x => x.CategoryId.ToString() == categoryId.ToString())
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
    }
}

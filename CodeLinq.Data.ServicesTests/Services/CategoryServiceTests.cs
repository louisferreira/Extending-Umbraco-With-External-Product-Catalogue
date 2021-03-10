using CodeLinq.Data.Contracts.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Entities;
using CodeLinq.Data.Contracts.Interfaces.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Repositories;
using CodeLinq.Data.Contracts.Interfaces.Services;
using CodeLinq.Data.Services.Models;
using CodeLinq.Data.ServicesTests.Helper;
using CodeLinq.Data.ServicesTests.TestEntities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace CodeLinq.Data.Services.Services.Tests
{
    public class CategoryServiceTests : IDisposable
    {
        private CategoryService target = null;
        private Mock<IRepository<ICategory>> mockCategoryRepository = null;
        private Mock<IRepository<IProduct>> mockProductRepository = null;
        private Mock<IRepository<ICategoryProduct>> mockCategoryProductRepository = null;
        private Mock<IMediaService> mockMediaService = null;

        private List<Category> dummyCategories = null;
        private List<Product> dummyProducts = null;
        private List<CategoryProduct> dummyCategoryProducts = null;
        private List<Media> dummyMedia = null;


        // setup
        public CategoryServiceTests()
        {
            mockCategoryRepository = new Mock<IRepository<ICategory>>();
            mockProductRepository = new Mock<IRepository<IProduct>>();
            mockCategoryProductRepository = new Mock<IRepository<ICategoryProduct>>();
            mockMediaService = new Mock<IMediaService>();

            dummyCategories = DataFactory.SeedDummyData<Category>();
            dummyProducts = DataFactory.SeedDummyData<Product>();
            dummyCategoryProducts = DataFactory.SeedDummyData<CategoryProduct>();
            dummyMedia = DataFactory.SeedDummyData<Media>();

            SetupMocks();

            target = new CategoryService(mockCategoryRepository.Object, mockProductRepository.Object, mockCategoryProductRepository.Object, mockMediaService.Object);
        }

        // Tests
        [Fact()]
        public void ShouldGetAllCategories()
        {
            var data = target.Get();
            var expectedCount = 3;
            var actualCount = data.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact()]
        public void ShouldReturnCategoryById()
        {
            var data = target.Get(Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564"));
            var expectedName = "Test Category 1";
            var actualName = data?.Name;

            Assert.Equal(expectedName, actualName);
        }

        [Fact()]
        public void ShouldReturnCategoriesByFilter()
        {
            var data = target.Get(cat => cat.ParentCategoryId != null);
            var expectedCount = 2;
            var actualCount = data.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact()]
        public void ShouldInsert()
        {
            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Test Category 4",
                Description = "Test Category 4",
                ParentCategoryId = null
            };

            var result = target.Insert(newCategory);

            var expectedResultStatus = 0;
            var actualResultStatus = result.ResultCode;
            var expectedCount = 4;
            var actualCount = dummyCategories.Count();

            Assert.Equal(expectedResultStatus, actualResultStatus);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact()]
        public void ShouldUpdate()
        {
            var id = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var category = dummyCategories.First(x => x.Id.Equals(id));
            category.Name = "Test Category updated";
            var result = target.Update(category);

            var expectedResultStatus = 0;
            var actualResultStatus = result.ResultCode;

            category = dummyCategories.First(x => x.Id.Equals(id));

            var expectedName = "Test Category updated";
            var actualName = category.Name;

            Assert.Equal(expectedResultStatus, actualResultStatus);
            Assert.Equal(expectedName, actualName);
        }

        [Fact()]
        public void ShouldGetCategoriesByParentId()
        {
            var id = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var data = target.GetCategoriesByParentId(id);
            var expectedCount = 1;
            var actualCount = data.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact()]
        public void ShouldReturnRootCategories()
        {
            var data = target.GetRootCategories();
            var expectedCount = 1;
            var actualCount = data.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact()]
        public void ShouldDeleteById()
        {
            var id = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var result = target.Delete(id);

            var expectedResultStatus = 0;
            var actualResultStatus = result.ResultCode;
            var expectedCount = 2;
            var actualCount = dummyCategories.Count();

            Assert.Equal(expectedResultStatus, actualResultStatus);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact()]
        public void ShouldDeleteByEntity()
        {
            var id = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var category = dummyCategories.First(x => x.Id.Equals(id));
            var result = target.Delete(category);

            var expectedResultStatus = 0;
            var actualResultStatus = result.ResultCode;
            var expectedCount = 2;
            var actualCount = dummyCategories.Count();

            Assert.Equal(expectedResultStatus, actualResultStatus);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact()]
        public void ShouldGetProductsByCategoryId()
        {
            var id = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var data = target.GetProductsByCategoryId(id);
            var expectedCount = 2;
            var actualCount = data.Count();

            Assert.Equal(expectedCount, actualCount);

        }

        [Fact]
        public void ShouldGetAllMediaForCategoryId()
        {
            var id = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var data = target.GetMedia(id);
            var expected = 2;
            var actual = data.Count();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldGetMediaByType()
        {
            var id = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var mediaType = MediaType.Image;
            var data = target.GetMedia(id, mediaType);
            var expected = 1;
            var actual = data.Count();
            Assert.Equal(expected, actual);
        }

        // private methods
        private void SetupMocks()
        {
            // Category Setup
            #region Category Setup

            mockCategoryRepository
                       .Setup(repo => repo.Get()).Returns(dummyCategories.AsQueryable);

            mockCategoryRepository
                .Setup(repo => repo.Get(It.Is<Guid>(id => id == Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564"))))
                .Returns<Guid>(id => dummyCategories.First(x => x.Id.Equals(id)));

            mockCategoryRepository
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<ICategory, bool>>>()))
                .Returns((Expression<Func<ICategory, bool>> predicate) => FilterCategories(predicate));

            mockCategoryRepository
                .Setup(repo => repo.Insert(It.IsAny<ICategory>()))
                .Callback<ICategory>((cat) => dummyCategories.Add((Category)cat))
                .Returns(new OperationResult<ICategory>()
                {
                    ResultCode = 0,
                    Message = "OK",
                    OperationOutcome = OperationOutcome.Success
                });

            mockCategoryRepository
                .Setup(repo => repo.Update(It.IsAny<ICategory>()))
                .Callback<ICategory>((cat) =>
                {
                    var category = dummyCategories.First(x => x.Id.Equals(cat.Id));
                    category.Name = cat.Name;
                    category.Description = cat.Description;
                    category.ParentCategoryId = cat.ParentCategoryId;
                })
                .Returns(new OperationResult<ICategory>()
                {
                    ResultCode = 0,
                    Message = "OK",
                    OperationOutcome = OperationOutcome.Success
                });

            mockCategoryRepository
               .Setup(repo => repo.Delete(It.IsAny<ICategory>()))
               .Callback<ICategory>((cat) =>
               {
                   var category = dummyCategories.First(x => x.Id.Equals(cat.Id));
                   dummyCategories.Remove(category);
               })
               .Returns(new OperationResult<ICategory>()
               {
                   ResultCode = 0,
                   Message = "OK",
                   OperationOutcome = OperationOutcome.Success
               });

            mockCategoryRepository
               .Setup(repo => repo.Delete(It.IsAny<Guid>()))
               .Callback<object>((id) =>
               {
                   var category = dummyCategories.First(x => x.Id.Equals(id));
                   dummyCategories.Remove(category);
               })
               .Returns(new OperationResult<ICategory>()
               {
                   ResultCode = 0,
                   Message = "OK",
                   OperationOutcome = OperationOutcome.Success
               });

            #endregion

            // Product Setup
            #region Product Setup

            mockProductRepository
                    .Setup(repo => repo.Get(It.Is<Guid>(id => id == Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83"))))
                    .Returns<Guid>(id => dummyProducts.First(x => x.Id.Equals(id)));

            mockProductRepository
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<IProduct, bool>>>()))
                .Returns((Expression<Func<IProduct, bool>> predicate) => FilterProducts(predicate));

            #endregion

            // CategoryProduct Setup
            #region CategoryProduct Setup

            mockCategoryProductRepository
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<ICategoryProduct, bool>>>()))
                .Returns((Expression<Func<ICategoryProduct, bool>> predicate) => FilterCategoryProducts(predicate));

            #endregion

            // MediaService Setup
            #region MediaService Setup

            mockMediaService
                .Setup(serv => serv.Get(It.IsAny<Guid>()))
                .Returns<Guid>((id) =>
                {
                    return dummyMedia.AsQueryable().FirstOrDefault(x => x.Id.Equals(id));
                });

            mockMediaService
                .Setup(serv => serv.Get(It.IsAny<Guid>(), It.IsAny<EntityType>()))
                .Returns<Guid, EntityType>((id, type) =>
                {
                    return dummyMedia.AsQueryable().Where(x => x.EntityId.Equals(id) && x.EntityType.Equals(type));
                });

            mockMediaService
                .Setup(serv => serv.Get(It.IsAny<Guid>(), It.IsAny<EntityType>(), It.IsAny<MediaType>()))
                .Returns<Guid, EntityType, MediaType>((id, type, mediaType) =>
                {
                    return dummyMedia.AsQueryable().Where(x => x.EntityId.Equals(id) && x.EntityType.Equals(type) && x.MediaType.Equals(mediaType));
                });

            mockMediaService
                .Setup(serv => serv.Delete(It.IsAny<IMedia>()))
                .Returns<IMedia>((item) =>
                {
                    var media = dummyMedia.FirstOrDefault(x => x.EntityId.Equals(item.EntityId));
                    if (media != null)
                    {
                        dummyMedia.Remove(media);
                        return new OperationResult<IMedia>()
                        {
                            ResultCode = 0,
                            Message = "OK",
                            OperationOutcome = OperationOutcome.Success,
                            Entity = null
                        };
                    }
                    else
                    {
                        return new OperationResult<IMedia>()
                        {
                            ResultCode = 0,
                            Message = "Not Found",
                            OperationOutcome = OperationOutcome.NotFound,
                            Entity = null
                        };
                    }
                }); 

            #endregion
        }

        private IQueryable<ICategory> FilterCategories(Expression<Func<ICategory, bool>> predicate)
        {
            return (predicate is null)
                ? dummyCategories.AsQueryable()
                : dummyCategories.AsQueryable().Where(predicate);
        }

        private IQueryable<ICategoryProduct> FilterCategoryProducts(Expression<Func<ICategoryProduct, bool>> predicate)
        {
            return (predicate is null)
                ? dummyCategoryProducts.AsQueryable()
                : dummyCategoryProducts.AsQueryable().Where(predicate);
        }

        private IQueryable<IProduct> FilterProducts(Expression<Func<IProduct, bool>> predicate)
        {
            return (predicate is null)
                ? dummyProducts.AsQueryable()
                : dummyProducts.AsQueryable().Where(predicate);
        }


        // tear down
        public void Dispose()
        {
            target = null;
            mockCategoryRepository = null;
            mockProductRepository = null;
            mockCategoryProductRepository = null;
            mockMediaService = null;
            dummyCategories = null;
            dummyProducts = null;
            dummyCategoryProducts = null;
            dummyMedia = null;

        }
    }
}
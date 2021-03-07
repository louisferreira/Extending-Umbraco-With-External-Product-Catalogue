using CodeLinq.Data.Contracts.Interfaces.Entities;
using CodeLinq.Data.Contracts.Interfaces.Repositories;
using CodeLinq.Data.Contracts.Interfaces.Services;
using CodeLinq.Data.ServicesTests.Helper;
using CodeLinq.Data.ServicesTests.TestEntities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodeLinq.Data.Services.Services.Tests
{
    public class ProductServiceTests : IDisposable
    {
        private ProductService target = null;
        private Mock<IRepository<IProduct>> mockProductRepository = null;
        private Mock<IRepository<ICategoryProduct>> mockCategoryProductRepository = null;
        private Mock<IMediaService> mockMediaService = null;

        private List<Product> dummyProducts = null;
        private List<CategoryProduct> dummyCatProd = null;
        private List<Media> dummyMedia = null;

        // set up
        public ProductServiceTests()
        {
            mockProductRepository = new Mock<IRepository<IProduct>>();
            mockCategoryProductRepository = new Mock<IRepository<ICategoryProduct>>();
            mockMediaService = new Mock<IMediaService>();

            dummyProducts = DataFactory.SeedDummyData<Product>();
            dummyCatProd = DataFactory.SeedDummyData<CategoryProduct>();
            dummyMedia = DataFactory.SeedDummyData<Media>();

            SetupMocks();

            target = new ProductService(mockProductRepository.Object, mockCategoryProductRepository.Object, mockMediaService.Object);
        }

        // test

        [Fact]
        public void ShouldReturnAllProducts()
        {
            var data = target.Get();
            var expectedCount = 5;
            var actualCount = data.Count();

            Assert.Equal(expectedCount, actualCount);

        }

        [Fact]
        public void ShouldReturnProductById()
        {
            var id = Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83");
            var data = target.Get(id);

            Assert.NotNull(data);

        }


        private void SetupMocks()
        {
            // set up ProductRepo
            mockProductRepository
                .Setup(repo => repo.Get())
                .Returns(dummyProducts.AsQueryable());

            mockProductRepository
                .Setup(repo => repo.Get(It.Is<Guid>(p => p == Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83"))))
                .Returns<Guid>(id => dummyProducts.FirstOrDefault(x => x.Id.ToString() == id.ToString()));

            // set up CategoryProductsRepo

            // set up MediaService
        }

        // tear down
        public void Dispose()
        {
            target = null;
            mockProductRepository = null;
            mockCategoryProductRepository = null;
            mockMediaService = null;

            dummyProducts = null;
            dummyCatProd = null;
            dummyMedia = null;

        }
    }
}
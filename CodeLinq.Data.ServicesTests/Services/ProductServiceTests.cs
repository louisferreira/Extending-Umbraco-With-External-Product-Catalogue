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

            var expectedName = "Monkfish Fresh - Skin Off";
            var actualName = data.Name;
            Assert.Equal(expectedName, actualName);

        }

        [Fact]
        public void ShouldReturnProductsByFilter()
        {
            var data = target.Get(x => x.Price < 500);
            var expectedCount = 2;
            var actualCount = data.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void ShouldInsertNewProduct()
        {
            var product = new Product
            {
                Name = "New Product",
                Description = "New Product",
                Sku = "1234",
                Price = 99.99M,
                OutOfStock = false
            };

            var result = target.Insert(product);
            var expectedOutCome = OperationOutcome.Success;
            var actualOutcome = result.OperationOutcome;

            Assert.Equal(expectedOutCome, actualOutcome);

            var expectedCount = 6;
            var actualCount = dummyProducts.Count();
            Assert.Equal(expectedCount, actualCount);

            Assert.True(result.Entity.Id != null);
        }

        [Fact]
        public void ShouldUpdateAndReturnProduct()
        {
            var id = Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83");
            var product = dummyProducts.FirstOrDefault(x => x.Id.Equals(id));
            product.Name = "Monkfish Fresh - Skin Off - Updated";

            var result = target.Update(product);

            var expectedOutcome = OperationOutcome.Success;
            var actualOutcome = result.OperationOutcome;
            Assert.Equal(expectedOutcome, actualOutcome);

            Assert.True(result.Entity.Name == "Monkfish Fresh - Skin Off - Updated");
        }

        [Fact]
        public void ShouldReturnNotFoundOnUpdate()
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
            };
            var result = target.Update(product);
            var expectedOutcome = OperationOutcome.NotFound;
            var actualOutcome = result.OperationOutcome;

            Assert.Equal(expectedOutcome, actualOutcome);
            Assert.True(result.Entity is null);
        }

        [Fact]
        public void ShouldDeleteProductById()
        {
            var id = Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83");
            var result = target.Delete(id);
            var expectedOutcome = OperationOutcome.Success;
            var actualOutcome = result.OperationOutcome;

            Assert.Equal(expectedOutcome, actualOutcome);
            Assert.True(result.Entity is null);

            var expectedCount = 4;
            var actualCount = dummyProducts.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void ShouldReturnNotFoundOnDelete()
        {
            var id = Guid.NewGuid();
            var result = target.Delete(id);
            var expectedOutcome = OperationOutcome.NotFound;
            var actualOutcome = result.OperationOutcome;

            Assert.Equal(expectedOutcome, actualOutcome);
            Assert.True(result.Entity is null);

            var expectedCount = 5;
            var actualCount = dummyProducts.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void ShouldReturnAllProductsByCategoryId()
        {
            var catId = Guid.Parse("33f76c00-7e72-4f67-8ece-38681ce88bc3");
            var data = target.GetByCategoryId(catId);

            var expectedCount = 3;
            var actualCount = data.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void ShouldGetAllMediaForProductId()
        {
            var id = Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83");
            var data = target.GetMedia(id);
            var expected = 2;
            var actual = data.Count();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldGetMediaByType()
        {
            var id = Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83");
            var mediaType = MediaType.Image;
            var data = target.GetMedia(id, mediaType);
            var expected = 1;
            var actual = data.Count();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldThrowNotImplementedExceptionForVariants()
        {
            var id = Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83");
            Assert.Throws<NotImplementedException>(() => target.GetVariants(id));
        }

        private void SetupMocks()
        {
            // set up ProductRepo
            #region ProductRepository Setup
            
            mockProductRepository
               .Setup(repo => repo.Get())
               .Returns(dummyProducts.AsQueryable());

            mockProductRepository
                .Setup(repo => repo.Get(It.Is<Guid>(p => p == Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83"))))
                .Returns<Guid>(id => dummyProducts.FirstOrDefault(x => x.Id.Equals(id)));

            mockProductRepository
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<IProduct, bool>>>()))
                .Returns<Expression<Func<IProduct, bool>>>(predicate => FilterProducts(predicate));

            mockProductRepository
               .Setup(repo => repo.Insert(It.IsAny<IProduct>()))
               .Callback<IProduct>((prod) =>
               {
                   if (prod.Id == null)
                   {
                       prod.Id = Guid.NewGuid();
                   }
                   dummyProducts.Add((Product)prod);
               })
               .Returns<IProduct>((p) => new OperationResult<IProduct>()
               {
                   ResultCode = 0,
                   Message = "OK",
                   OperationOutcome = OperationOutcome.Success,
                   Entity = dummyProducts.First(x => x.Sku == p.Sku && x.Name == p.Name)
               });

            mockProductRepository
                .Setup(repo => repo.Update(It.IsAny<IProduct>()))
                .Returns<IProduct>((prod) =>
                {
                    var Product = dummyProducts.FirstOrDefault(x => x.Id.ToString() == prod.Id.ToString());
                    if (Product != null)
                    {
                        Product.Name = prod.Name;
                        Product.Description = prod.Description;
                        Product.Sku = prod.Sku;
                        Product.Price = prod.Price;
                        Product.OutOfStock = prod.OutOfStock;

                        return new OperationResult<IProduct>()
                        {
                            ResultCode = 0,
                            Message = "OK",
                            OperationOutcome = OperationOutcome.Success,
                            Entity = prod
                        };
                    }
                    else
                    {
                        return new OperationResult<IProduct>()
                        {
                            ResultCode = 0,
                            Message = "OK",
                            OperationOutcome = OperationOutcome.NotFound,
                            Entity = null
                        };
                    }

                });

            mockProductRepository
               .Setup(repo => repo.Delete(It.IsAny<Guid>()))
               .Returns<Guid>((id) =>
               {
                   var product = dummyProducts.FirstOrDefault(x => x.Id.Equals(id));
                   if (product != null)
                   {
                       dummyProducts.Remove(product);
                       return new OperationResult<IProduct>()
                       {
                           ResultCode = 0,
                           Message = "OK",
                           OperationOutcome = OperationOutcome.Success,
                           Entity = null
                       };
                   }
                   else
                   {
                       return new OperationResult<IProduct>()
                       {
                           ResultCode = 0,
                           Message = "Not Found",
                           OperationOutcome = OperationOutcome.NotFound,
                           Entity = null
                       };
                   }
               });


            #endregion

            // set up CategoryProductsRepo
            #region CategoryProduct Setups
            
            mockCategoryProductRepository
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<ICategoryProduct, bool>>>()))
                .Returns<Expression<Func<ICategoryProduct, bool>>>((predicate) => FilterCategoryProducts(predicate));

            mockCategoryProductRepository
                .Setup(repo => repo.Delete(It.IsAny<Guid>()))
                .Returns<Guid>((id) =>
                {
                    var item = dummyCatProd.FirstOrDefault(x => x.Id.Equals(id));
                    if (item != null)
                    {
                        dummyCatProd.Remove(item);
                        return new OperationResult<ICategoryProduct>()
                        {
                            ResultCode = 0,
                            Message = "OK",
                            OperationOutcome = OperationOutcome.Success,
                            Entity = null
                        };
                    }
                    else
                    {
                        return new OperationResult<ICategoryProduct>()
                        {
                            ResultCode = 0,
                            Message = "Not Found",
                            OperationOutcome = OperationOutcome.NotFound,
                            Entity = null
                        };
                    }

                });

            #endregion

            // set up MediaService
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

        private IQueryable<ICategoryProduct> FilterCategoryProducts(Expression<Func<ICategoryProduct, bool>> predicate)
        {
            return predicate is null
            ? dummyCatProd.AsQueryable()
            : dummyCatProd.AsQueryable().Where(predicate);
        }

        private IQueryable<IProduct> FilterProducts(Expression<Func<IProduct, bool>> predicate)
        {
            return predicate is null
                ? dummyProducts.AsQueryable()
                : dummyProducts.AsQueryable().Where(predicate);
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
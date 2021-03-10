using Moq;
using CodeLinq.Data.Services.Models;
using CodeLinq.Data.Services.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using CodeLinq.Data.Contracts.Interfaces.Repositories;
using CodeLinq.Data.Contracts.Interfaces.Entities;
using CodeLinq.Data.Contracts.Interfaces.Providers;
using CodeLinq.Data.ServicesTests.Helper;
using CodeLinq.Data.Contracts.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Infrastructure;

namespace CodeLinq.Data.Services.Services.Tests
{
    public class MediaServiceTests : IDisposable
    {
        private MediaService target = null;
        private Mock<IRepository<IMedia>> mockMediaRepo = null;
        private Mock<IFileSystemProvider> mockFilesystemProvider = null;
        private List<Media> dummyMediaRepo = null;

        // set up
        public MediaServiceTests()
        {
            mockMediaRepo = new Mock<IRepository<IMedia>>();
            mockFilesystemProvider = new Mock<IFileSystemProvider>();
            target = new MediaService(mockMediaRepo.Object, mockFilesystemProvider.Object);
            dummyMediaRepo = DataFactory.SeedDummyData<Media>();
            SetupMocks();
        }

        //tests
        [Fact]
        public void ShouldASingleMediaById()
        {
            var id = Guid.Parse("75750707-f77d-4899-85e1-cedae9ac8bff");
            var data = target.Get(id);
            Assert.NotNull(data);
        }

        [Fact]
        public void ShouldReturnAllMediaForProduct()
        {
            var id = Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83");
            var data = target.Get(id, EntityType.Product);
            var expectedCount = 2;
            var actualCount = data.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void ShouldReturnAllMediaForProductByType()
        {
            var id = Guid.Parse("b4f21064-3c3f-459a-b2e4-77e982b3fe83");
            var data = target.Get(id, EntityType.Product, MediaType.Image);
            var expectedCount = 1;
            var actualCount = data.Count();
            Assert.Equal(expectedCount, actualCount);

            data = target.Get(id, EntityType.Product, MediaType.Instructions);
            expectedCount = 1;
            actualCount = data.Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void ShouldDeleteAFileThatExists()
        {
            var file = "fileThatExists.png";
            var media = new Media
            {
                EntityId = Guid.NewGuid(),
                EntityType = EntityType.Product,
                FileExtension = "",
                FilePath = file,
                Id = Guid.NewGuid()
            };
            var result = target.Delete(media);

            var expectedOutcome = OperationOutcome.Success;
            var actualOutcome = result.OperationOutcome;
            Assert.Equal(expectedOutcome, actualOutcome);

        }

        [Fact]
        public void ShouldNotDeleteAFileThatDoesntExists()
        {
            var file = "fileThatDoesntExists.png";
            var media = new Media
            {
                EntityId = Guid.NewGuid(),
                EntityType = EntityType.Product,
                FileExtension = "",
                FilePath = file,
                Id = Guid.NewGuid()
            };
            var result = target.Delete(media);

            var expectedOutcome = OperationOutcome.NotFound;
            var actualOutcome = result.OperationOutcome;
            Assert.Equal(expectedOutcome, actualOutcome);

        }

        [Fact]
        public void ShouldNotDeleteAFileThatCannotBeDeleted()
        {
            var file = "fileThatThrowsError.png";
            var media = new Media
            {
                EntityId = Guid.NewGuid(),
                EntityType = EntityType.Product,
                FileExtension = "",
                FilePath = file,
                Id = Guid.NewGuid()
            };
            var result = target.Delete(media);

            var expectedOutcome = OperationOutcome.InternalError;
            var actualOutcome = result.OperationOutcome;
            Assert.Equal(expectedOutcome, actualOutcome);

        }

        [Fact]
        public void ShouldReturnMediaByCategoryId()
        {
            var catId = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var result = target.Get(catId, EntityType.Category);
            var expectedCount = 2;
            var actualcount = result.Count();
            Assert.Equal(expectedCount, actualcount);
        }

        [Fact]
        public void ShouldReturnNoMediaByNonExistentCategoryId()
        {
            var catId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var result = target.Get(catId, EntityType.Category);
            var expectedCount = 0;
            var actualcount = result.Count();
            Assert.Equal(expectedCount, actualcount);
        }

        [Fact]
        public void ShouldReturnCategoryByMediaTypeWhenExists()
        {
            var catId = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var result = target.Get(catId, EntityType.Category, MediaType.Video);
            var expectedCount = 1;
            var actualcount = result.Count();
            Assert.Equal(expectedCount, actualcount);
        }

        [Fact]
        public void ShouldReturnCategoryByMediaTypeWhenNotExists()
        {
            var catId = Guid.Parse("720f448a-22b8-439b-8cd2-3a5b0ca21564");
            var result = target.Get(catId, EntityType.Category, MediaType.PDF);
            var expectedCount = 0;
            var actualcount = result.Count();
            Assert.Equal(expectedCount, actualcount);
        }

        [Fact]
        public void ShouldSaveFileSuccessfully()
        {
            var data = new byte[100];
            var uploadedFileName = "test.png";
            var entityId = Guid.NewGuid();
            var result = target.Save(data, uploadedFileName, entityId, EntityType.Product, MediaType.Image);

            var expectedOutcomeResult = OperationOutcome.Success;
            var expectedFileName = "test.png";
            var expectedFileExt = ".png";
            var expectedMimeType = "image/png";
            var expectedFileSize = 100;
            var expectedTentityType = EntityType.Product;
            var expectedMediaType = MediaType.Image;

            Assert.Equal(expectedOutcomeResult, result.OperationOutcome);
            Assert.Equal(expectedFileName, result.Entity.Name);
            Assert.Equal(expectedFileExt, result.Entity.FileExtension);
            Assert.Equal(expectedMimeType, result.Entity.MimeType);
            Assert.Equal(expectedFileSize, result.Entity.FileSize);
            Assert.Equal(expectedTentityType, result.Entity.EntityType);
            Assert.Equal(expectedMediaType, result.Entity.MediaType);

        }

        [Fact]
        public void ShouldReturnCorrectOperationResultIfNoData()
        {
            var data = new byte[0];
            var uploadedFileName = "test.png";
            var entityId = Guid.NewGuid();
            var result = target.Save(data, uploadedFileName, entityId, EntityType.Product, MediaType.Image);
            Assert.True(result.OperationOutcome == OperationOutcome.InternalError);
            Assert.Equal("Value cannot be null. (Parameter 'data')", result.Message);

        }

        [Fact]
        public void ShouldReturnCorrectOperationResultIfNoFileName()
        {
            var data = new byte[10];
            var uploadedFileName = string.Empty;
            var entityId = Guid.NewGuid();
            var result = target.Save(data, uploadedFileName, entityId, EntityType.Product, MediaType.Image);
            Assert.True(result.OperationOutcome == OperationOutcome.InternalError);
            Assert.Equal("Value cannot be null. (Parameter 'uploadedFileName')", result.Message);

        }

        [Fact]
        public void ShouldReturnCorrectOperationResultIfNoEntityId()
        {
            var data = new byte[10];
            var uploadedFileName = "testfile.pmg";
            object entityId = null;
            var result = target.Save(data, uploadedFileName, entityId, EntityType.Product, MediaType.Image);
            Assert.True(result.OperationOutcome == OperationOutcome.InternalError);
            Assert.Equal("Value cannot be null. (Parameter 'entityId')", result.Message);

        }

        [Fact]
        public void ShouldReturnCorrectOperationResultIfNoFileExtension()
        {
            var data = new byte[10];
            var uploadedFileName = "testfile";
            object entityId = null;
            var result = target.Save(data, uploadedFileName, entityId, EntityType.Product, MediaType.Image);
            Assert.True(result.OperationOutcome == OperationOutcome.InternalError);
            Assert.Equal("'uploadedFileName' param does not contain an extension", result.Message);

        }


        //private methods
        private void SetupMocks()
        {
            // IRepository<IMedia> setup
            mockMediaRepo
                .Setup(repo => repo.Get(It.IsAny<Guid>()))
                .Returns<Guid>(id => dummyMediaRepo.First(x => x.Id.Equals(id)));

            mockMediaRepo
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<IMedia, bool>>>()))
                .Returns((Expression<Func<IMedia, bool>> predicate) => Filter(predicate));

            mockMediaRepo
                .Setup(repo => repo.Delete(It.Is<IMedia>(p => p.FilePath == "fileThatExists.png")))
                .Returns(() => new OperationResult<IMedia>
                {
                    Entity = null,
                    OperationOutcome = OperationOutcome.Success
                });

            mockMediaRepo
                .Setup(repo => repo.Delete(It.Is<IMedia>(p => p.FilePath == "fileThatDoesntExists.png")))
                .Returns<IMedia>((m) => new OperationResult<IMedia>
                {
                    Entity = m,
                    OperationOutcome = OperationOutcome.NotFound,
                    Message = "File Not found",
                    ResultCode = -1
                });

            mockMediaRepo
                .Setup(repo => repo.Insert(It.IsAny<IMedia>()))
                .Returns<IMedia>((m) =>
                {
                    m.Id = Guid.NewGuid();
                    return new OperationResult<IMedia>
                    {
                        Entity = m,
                        OperationOutcome = OperationOutcome.Success,
                        ResultCode = 0,
                        Message = "ok"
                    };
                });

            mockMediaRepo
                .Setup(repo => repo.Update(It.IsAny<IMedia>()))
                .Returns<IMedia>((m) => {
                    m.Id = Guid.NewGuid();
                    return new OperationResult<IMedia>
                    {
                        Entity = m,
                        OperationOutcome = OperationOutcome.Success,
                        ResultCode = 0,
                        Message = "ok"
                    };
                });

            //IFileSystemProvider setup
            mockFilesystemProvider
                .Setup(prov => prov.FileExits(It.Is<string>(p => p == "fileThatExists.png")))
                .Returns(true);

            mockFilesystemProvider
                .Setup(prov => prov.FileExits(It.Is<string>(p => p == "fileThatThrowsError.png")))
                .Returns(true);

            mockFilesystemProvider
                .Setup(prov => prov.FileExits(It.Is<string>(p => p == "fileThatDoesntExists.png")))
                .Returns(false);

            mockFilesystemProvider
                .Setup(prov => prov.DeleteFile(It.Is<string>(p => p == "fileThatExists.png")))
                .Returns(true);

            mockFilesystemProvider
                .Setup(prov => prov.DeleteFile(It.Is<string>(p => p == "fileThatDoesntExists.png")))
                .Returns(false);

            mockFilesystemProvider
                .Setup(prov => prov.DeleteFile(It.Is<string>(p => p == "fileThatThrowsError.png")))
                .Returns(false);

            mockFilesystemProvider
                .Setup(prov => prov.StoreFile(It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns<byte[], string>((data, file) => String.Concat("C:\\temp\\test\\", file));


        }

        private IQueryable<IMedia> Filter(Expression<Func<IMedia, bool>> predicate)
        {
            return (predicate != null)
                ? dummyMediaRepo.AsQueryable().Where(predicate)
                : dummyMediaRepo.AsQueryable();
        }

        // Teardown
        public void Dispose()
        {
            mockMediaRepo = null;
            mockFilesystemProvider = null;
            dummyMediaRepo = null;
            target = null;
        }
    }
}
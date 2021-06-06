using CodeLinq.Data.Contracts.Interfaces.Providers;
using CodeLinq.Data.Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodeLinq.Data.ServicesTests.Providers
{
    public class DefaultFileSystemProviderTests : IDisposable
    {
        private IFileSystemProvider target;

        // variables for creating a dummy test file
        readonly string folderLocation = null;
        private const string validFile = "test media.mp4";
        private const string validFileLegalCopy = "test media(1).mp4";
        private const string inValidFile = "not exist.mp4";
        private const int validFileSize = 500000;

        // setup
        public DefaultFileSystemProviderTests()
        {
            // ask the operating system to give us a temporary folder to work with
            folderLocation = System.IO.Path.GetTempPath() + "media\\";
            // create a known dummy file we can test against
            CreateExistingDummyFile(folderLocation);
            // instantiate the target with prefered options
            target = new DefaultFileSystemProvider(options =>
            {
                options.RootFolderLocation = folderLocation;
                options.ValidFileExtensions = new[] { ".mp4", ".tmp" };
                // We will use the default SubFolderRule built-in property for most tests
            });
        }


        // tests
        [Fact()]
        public void ShouldThrowExceptionIfNoOptionsSpecified()
        {
            // if option is null in constructor, we expect a particular exception type
            Assert.Throws<ArgumentNullException>(() =>
            {
                FileSystemProviderOptions option = null;
                target = new DefaultFileSystemProvider(option);
            });
        }

        [Fact()]
        public void ShouldThrowExceptionIfNoRootFolderSpecified()
        {
            // if no root folder is specified inthe options, we expect a particular exception type
            Assert.Throws<InvalidOperationException>(() =>
            {
                target = new DefaultFileSystemProvider(options =>
                {
                    options.ValidFileExtensions = new[] { ".mp4", ".tmp" };
                });
            });
        }

        [Fact()]
        public void ShouldRetrieveAFileThatExists()
        {
            // lets ask the target to return us the known dummy file and validate it does so correctly
            var dummyValidFile = "\\mp4\\" + validFile;
            var byteArray = target.RetrieveFile(dummyValidFile);

            // does it return something
            Assert.NotNull(byteArray);

            // is the file size returned as expected
            var expectedFileSize = validFileSize;
            var actualFileSize = byteArray.Length;
            Assert.Equal(expectedFileSize, actualFileSize);
        }

        [Fact()]
        public void ShouldReturnNullIfFileNotExists()
        {
            // if the file does not exist, does it return null?
            var data = target.RetrieveFile(inValidFile);
            Assert.Null(data);
        }

        [Fact()]
        public void ShouldReturnTrueIfFileExists()
        {
            // when asked if a file exists, does it return true for existing file?
            var dummyValidFile = folderLocation + "\\mp4\\" + validFile;
            Assert.True(target.FileExits(dummyValidFile));
        }

        [Fact()]
        public void ShouldReturnFalseIfFileNotExists()
        {
            // when asked if a file doesn't exists, does it return false for a non-existing file?
            Assert.False(target.FileExits(inValidFile));
        }

        [Fact()]
        public void ShouldReturnAvailableFileName()
        {
            // if a file with the specified name already exists, will it return an indexed appended filename?
            var actualName = target.GetNextAvailableFileName(validFile);
            var expectedName = validFileLegalCopy;

            Assert.Equal(expectedName, actualName);
        }

        [Fact()]
        public void ShouldThrowExceptionOnGetAvailableFileNameIfPath()
        {
            // if we ask that a file exists with a folder in the path, does it throw a specific exception?
            var path = folderLocation + validFile;
            Assert.Throws<ArgumentException>(() => target.GetNextAvailableFileName(path));
        }

        [Fact()]
        public void ShouldStoreFileAndReturnFilename()
        {
            //create a known file
            var data = new byte[validFileSize];
            var fileName = System.IO.Path.GetTempFileName();
            fileName = System.IO.Path.GetFileName(fileName);

            // ask the target to save it
            var actual = target.StoreFile(data, fileName);
            var exptected = folderLocation + "tmp\\" + fileName;

            // does it return the correct filename with path?
            Assert.Equal(exptected, actual);
        }

        [Fact()]
        public void ShouldStoreFileAndReturnFilenameWithSubFolderRule()
        {
            // create a known file
            var data = new byte[validFileSize];
            var fileName = System.IO.Path.GetTempFileName();
            fileName = System.IO.Path.GetFileName(fileName);

            // add our own rule
            target.SubFolderRule = CreateSubFolder;

            // ask the target to save the file
            var actual = target.StoreFile(data, fileName);
            var exptected = folderLocation + DateTime.Now.DayOfYear.ToString("X2") + "\\" + fileName;

            // does it return the correct file name and path (using the sub folder rule)?
            Assert.Equal(exptected, actual);
        }

        [Fact()]
        public void ShouldDeleteAFileThatExists()
        {
            // create a known file
            var data = new byte[validFileSize];
            var fileName = System.IO.Path.GetTempFileName();
            fileName = System.IO.Path.GetFileName(fileName);

            // ask the target to save it
            var actual = target.StoreFile(data, fileName);
            var exptected = folderLocation + "tmp\\" + fileName;

            // check it was saved correctly
            Assert.Equal(exptected, actual);

            // now ask the target to delete it, and verify it suceeded
            var succeeded = target.DeleteFile(actual);
            Assert.True(succeeded);

            // now verify that it was indeed deleted
            Assert.False(target.FileExits(fileName));
        }

        [Fact]
        public void ShouldThrowInvalidOperationExceptionForIncorrectFileType()
        {
            // try to store a file that is not in the allowed file extensions list
            var fileName = "invalid.exe";

            // verify that the correct exception was thrown
            Assert.Throws<InvalidOperationException>(() => target.StoreFile(null, fileName));
        }


        //private methods
        private string CreateSubFolder(string root, string fileName)
        {
            // a rule that creates a sub folder with the 2 digit year in the path
            return root + DateTime.Now.DayOfYear.ToString("X2") + "\\";
        }
        private void CreateExistingDummyFile(string folderLocation)
        {
            // create dummy test file to run tests against.
            byte[] data = new byte[validFileSize];

            var dummyFileLocation = folderLocation + "\\mp4\\";
            if (!System.IO.Directory.Exists(dummyFileLocation))
            {
                System.IO.Directory.CreateDirectory(dummyFileLocation);
            }

            // write the file out to disk
            System.IO.File.WriteAllBytes(dummyFileLocation + validFile, data);
        }

        // tear down
        public void Dispose()
        {
            // clean up the test folder after use
            target = null;
            System.IO.Directory.Delete(folderLocation, true);
        }
    }
}

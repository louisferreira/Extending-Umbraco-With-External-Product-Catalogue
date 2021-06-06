using CodeLinq.Data.Contracts.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeLinq.Data.Services.Providers
{
    public class DefaultFileSystemProvider : IFileSystemProvider
    {
        private readonly FileSystemProviderOptions settings;
        private SemaphoreSlim threadGate = new SemaphoreSlim(1);

        public string RootFolderLocation => settings.RootFolderLocation;
        public string[] ValidFileExtensions => settings.ValidFileExtensions;
        public Func<string, string, string> SubFolderRule
        {
            get => settings.SubFolderRule;
            set => settings.SubFolderRule = value;
        }

        public DefaultFileSystemProvider(Action<FileSystemProviderOptions> options)
        {
            settings = new FileSystemProviderOptions();
            options(settings);
            ApplySettings(settings);
        }

        public DefaultFileSystemProvider(FileSystemProviderOptions settings)
        {
            this.settings = settings;
            ApplySettings(settings);
        }

        private void ApplySettings(FileSystemProviderOptions settings)
        {
            if (settings is null)
                throw new ArgumentNullException("'FileSystemProviderOptions' parameter cannot be null.");

            if (!string.IsNullOrWhiteSpace(settings.RootFolderLocation))
                settings.RootFolderLocation = settings.RootFolderLocation;
            else
                throw new InvalidOperationException("'RootFolderLocation' property is required for DefaultFileSystemProvider. " +
                    "Please specify folder in FileSystemProviderOptions parameter of constructor.");

            if (settings.ValidFileExtensions != null && settings.ValidFileExtensions.Length > 0)
                settings.ValidFileExtensions = settings.ValidFileExtensions;
            else
                settings.ValidFileExtensions = GetDefaultValidExtensions();

            if (settings.SubFolderRule != null)
                settings.SubFolderRule = settings.SubFolderRule;
            else
                settings.SubFolderRule = DefaultSubFolder;
        }

        public byte[] RetrieveFile(string fileName)
        {
            if (!this.FileExits(this.RootFolderLocation + fileName))
                return null;
            try
            {
                threadGate.Wait(); // block thread if there is already a thread in this section
                return System.IO.File.ReadAllBytes(this.RootFolderLocation + fileName);
            }
            catch (Exception ex)
            {
                // log the error here
                throw;
            }
            finally
            {
                threadGate.Release(); // notify waiting threads that another one can enter
            }
        }

        public string StoreFile(byte[] fileData, string originalFileName)
        {
            // check if file extension is allowed
            var fileExtension = System.IO.Path.GetExtension(originalFileName);
            if (!ValidFileExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException($"Saving files of type '{fileExtension}' is not supported");
            }

            try
            {
                threadGate.Wait(); // block thread if there is already a thread in this section
                string location = GetSubFolderFromRule(originalFileName);

                // check if the filename already exists in that location
                var fileName = GetNextAvailableFileName(originalFileName);
                var fullPath = System.IO.Path.Combine(location, fileName);

                // write the file data out
                System.IO.File.WriteAllBytes(fullPath, fileData);


                return fullPath;
            }
            catch (Exception ex)
            {
                //log the error here
                throw;
            }
            finally
            {
                threadGate.Release(); // notify waiting threads that another one can enter
            }
        }

        public bool DeleteFile(string fileName)
        {
            if (this.FileExits(fileName))
            {
                try
                {
                    threadGate.Wait(); // block thread if there is already a thread in this section
                    System.IO.File.Delete(fileName);
                    return true;
                }
                catch (Exception ex)
                {
                    // log the error here
                    throw;
                }
                finally
                {
                    threadGate.Release(); // notify waiting threads that another one can enter
                }
            }
            return false;
        }

        public bool FileExits(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;
            return System.IO.File.Exists(fileName);
        }

        public string GetNextAvailableFileName(string uploadedFileName)
        {
            if (!string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(uploadedFileName)))
                throw new ArgumentException("'UploadedFileName' must be a file without a directory path.", "uploadedFileName");

            var location = GetSubFolderFromRule(uploadedFileName);
            var fullPath = System.IO.Path.Combine(location, uploadedFileName);
            if (this.FileExits(fullPath))
            {
                var index = 1;
                var fileNameOnly = System.IO.Path.GetFileNameWithoutExtension(uploadedFileName);
                var fileExt = System.IO.Path.GetExtension(uploadedFileName);
                var checkThisFileName = $"{fileNameOnly}({index}){fileExt}";

                while (this.FileExits(checkThisFileName))
                {
                    index++;
                    checkThisFileName = $"{fileNameOnly}({index}){fileExt}";
                }
                uploadedFileName = checkThisFileName;
            }

            return uploadedFileName;
        }


        private string GetSubFolderFromRule(string originalFileName)
        {
            // if a sub folder rule is specified, get the folder location
            var location = settings.RootFolderLocation;
            if (SubFolderRule != null)
            {
                location = SubFolderRule(settings.RootFolderLocation, originalFileName);
                if (!location.EndsWith("\\"))
                    location = location + "\\";
                if (!System.IO.Directory.Exists(location))
                {
                    System.IO.Directory.CreateDirectory(location);
                }
            }

            return location;
        }

        private string[] GetDefaultValidExtensions()
        {
            return new[] { ".png", ".jpeg", ".jpg", ".gif", ".pdf", ".mp4", ".mov", ".ogg", ".svg" };
        }

        private string DefaultSubFolder(string root, string fileName)
        {
            //default sub folder rule creates a sub folder based on the file's extension.
            var fileExtension = System.IO.Path.GetExtension(fileName);
            if (fileExtension.StartsWith("."))
                fileExtension = fileExtension.TrimStart('.');
            return System.IO.Path.Combine(root, fileExtension);
        }
    }


}

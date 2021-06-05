using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLinq.Data.Contracts.Interfaces.Providers
{
    /// <summary>
    /// An definition for storing files to a physical location
    /// </summary>
    public interface IFileSystemProvider
    {
        /// <summary>
        /// Returns the physical or network location of the root folder;
        /// </summary>
        string RootFolderLocation { get; }

        /// <summary>
        /// Gets a list of allowed file extensions the provider can save.
        /// </summary>
        /// <returns>A string array containing valid file extensions that are allowed to be saved.</returns>
        string[] ValidFileExtensions { get; }

        /// <summary>
        /// Gets or Sets a function that will be called if subfolder rules are to be applied. First argument is the root folder, second argument is the filename.
        /// The returned string should be the final folder location (without file name) where the file is to be saved.
        /// Default rule will create a subfolder with the same name as the file extension.
        /// </summary>
        Func<string, string, string> SubFolderRule { get; set; }

        /// <summary>
        /// Stores file data to a physical location. If a SubFolderRule property value is set, then the file will be created using that rule.
        /// </summary>
        /// <param name="fileData">The data in the form of a byte array to store</param>
        /// <param name="originalFileName">The original file name of the data</param>
        /// <returns>A string containing the full path to the stored file.</returns>
        string StoreFile(byte[] fileData, string originalFileName);

        /// <summary>
        /// Retrieves a file from the physical location
        /// </summary>
        /// <param name="fileName">The full path and name of the file to retrieve relative to the root folder.</param>
        /// <returns>A byte array containing the file data</returns>
        byte[] RetrieveFile(string fileName);

        /// <summary>
        /// Deletes a file from the physical location
        /// </summary>
        /// <param name="fileName">The full path and name of the file to delete relative to the root folder.</param>
        /// <returns>True if the file was deleted succesfully, otherwise false.</returns>
        bool DeleteFile(string fileName);

        /// <summary>
        /// Gets a value indicating if a file exists.
        /// </summary>
        /// <param name="fileName">The full path and name of the file to check relative to the root folder.</param>
        /// <returns>True if the file exists, otherwise False.</returns>
        bool FileExits(string fileName);

        /// <summary>
        /// Returns a new file name if the specified file name already exists in the location by appending a bracketed index to the end.
        /// </summary>
        /// <remarks>
        /// If a file named 'file.png' exists, then this method returns 'file(1).png'
        /// </remarks>
        /// <param name="uploadedFileName">The file name to check. RootFolderLocation and SubFolderRules will be used to determine the location to check./param>
        /// <returns>A string containing the new file name, or the original file name if it doesn't already exist.</returns>
        string GetAvailableFileName(string uploadedFileName);
    }
}
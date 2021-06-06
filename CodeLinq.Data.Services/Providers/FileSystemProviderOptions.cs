using System;

namespace CodeLinq.Data.Services.Providers
{
    public class FileSystemProviderOptions
    {
        private string[] validFileExtensions;
        private string rootFolderLocation;

        /// <summary>
        /// Gets or sets a delegate function (or Lambda) to perform a rule the produces a new sub folder name. First argument is the root folder, second argument is the original file name.
        /// </summary>
        public Func<string, string, string> SubFolderRule { get; set; }

        /// <summary>
        /// Gets or sets the Root folder location for the FileSystemProvider.
        /// </summary>
        public string RootFolderLocation
        {
            get => rootFolderLocation;
            set
            {
                if (!value.EndsWith("\\"))
                    value += "\\";
                rootFolderLocation = value;
            }
        }

        /// <summary>
        /// Gets or sets an array of allowed file extensions. E.g. {".txt", ".mp4", ".png"}
        /// </summary>
        public string[] ValidFileExtensions
        {
            get => validFileExtensions;
            set
            {
                for (int index = 0; index < value.Length; index++)
                {
                    if (!value[index].StartsWith("."))
                        value[index] = "." + value[index];
                }
                validFileExtensions = value;
            }
        }
    }
}
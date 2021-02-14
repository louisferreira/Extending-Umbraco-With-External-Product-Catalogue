using CodeLinq.Data.Contracts.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Entities.Base;
using CodeLinq.Data.Contracts.Interfaces.Infrastruture;

namespace CodeLinq.Data.Contracts.Interfaces.Entities
{
    /// <summary>
    /// Represents a media file associated to an entity
    /// </summary>
    public interface IMedia : IUniqueIdentifier
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity that this media items belongs to
        /// </summary>
        object EntityId { get; set; }
        /// <summary>
        /// Gets or sets the type of media for this entity
        /// </summary>
        MediaType MediaType { get; set; }
        /// <summary>
        /// Gets or set the Entity Type that this media items belongs to
        /// </summary>
        EntityType EntityType { get; set; }
        /// <summary>
        /// Gets or sets the File Path for this media item
        /// </summary>
        string FilePath { get; set; }
        /// <summary>
        /// Gets or sets the number of bytes for this media entity
        /// </summary>
        long FileSize { get; set; }
        /// <summary>
        /// Gets or set the name of the media item
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets or set the mine type for this media item
        /// </summary>
        string MimeType { get; set; }
        /// <summary>
        /// Gets or set the file extension of this media item
        /// </summary>
        string FileExtension { get; set; }

    }
}

using CodeLinq.Data.Contracts.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Entities;
using CodeLinq.Data.Contracts.Interfaces.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Infrastruture;
using System.Collections.Generic;

namespace CodeLinq.Data.Contracts.Interfaces.Services
{
    /// <summary>
    /// An Interface for a Service that contains methods to perform operations on IMedia
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// A method to return a single IMedia instance
        /// </summary>
        /// <param name="id">The unique identifier of the entity to return</param>
        /// <returns>A IMedia instance</returns>
        IMedia Get(object id);

        /// <summary>
        /// A method to return all IMedia entities belonging to an entity of a specified type
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity</param>
        /// <param name="entityType">The EntityType of the that the id belongs to</param>
        /// <returns>An IEnumerable<IMedia> collection</returns>
        IEnumerable<IMedia> Get(object entityId, EntityType entityType);

        /// <summary>
        /// A method to return all IMedia entities belonging to an entity of a specified type, and of a specified media type
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity</param>
        /// <param name="entityType">The EntityType of the that the id belongs to</param>
        /// <param name="mediaType">The MediaType to filter by</param>
        /// <returns>An IEnumerable<IMedia> collection</returns>
        IEnumerable<IMedia> Get(object entityId, EntityType entityType, MediaType mediaType);

        /// <summary>
        /// A method to create a IMedia entity, and a physical file on the storage location.
        /// </summary>
        /// <param name="data">The byte array of the media file to save</param>
        /// <param name="fileName">The name of the original file</param>
        /// <param name="entityId">The unique identifier that this media item belongs to</param>
        /// <param name="entityType">The type of entity that this media item belongs to</param>
        /// <returns>An IOperationResult<IMedia> object containing the results of the operation</returns>
        IOperationResult<IMedia> Save(byte[] data, string fileName, object entityId, EntityType entityType, MediaType mediaType);

        /// <summary>
        /// A method that deletes the IMedia instance from the data store, and any physical files on storage.
        /// </summary>
        /// <param name="mediaId">The unique identifier of the IMedia item to delete</param>
        /// <returns>An IOperationResult<IMedia> object containing the results of the operation</returns>
        IOperationResult<IMedia> Delete(object mediaId);

        /// <summary>
        /// A method that deletes the IMedia instance from the data store, and any physical files on storage.
        /// </summary>
        /// <param name="media">The IMedia item to delete</param>
        /// <returns>An IOperationResult<IMedia> object containing the results of the operation</returns>
        IOperationResult<IMedia> Delete(IMedia media);
    }
}

namespace CodeLinq.Data.Contracts.Interfaces.Entities.Base
{
    /// <summary>
    /// Defines a contract that specifies that an entity object must have a unique identifier field.
    /// </summary>
    public interface IUniqueIdentifier
    {
        /// <summary>
        /// Gets or sets a value indicating the unique identifier field for an entity.
        /// </summary>
        object Id { get; set; }
    }
}

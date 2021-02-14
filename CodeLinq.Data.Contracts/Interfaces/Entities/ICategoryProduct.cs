using CodeLinq.Data.Contracts.Interfaces.Entities.Base;

namespace CodeLinq.Data.Contracts.Interfaces.Entities
{
    /// <summary>
    /// Defines a contract that specifie a relationship mapping between ICategory and IProduct
    /// </summary>
    public interface ICategoryProduct : IUniqueIdentifier
    {
        /// <summary>
        /// The UniqueIdentifier of the ICategory entity this mapping relates to
        /// </summary>
        object CategoryId { get; set; }
        /// <summary>
        /// The UniqueIdentifier of the IProduct entity this mapping relates to
        /// </summary>
        object ProductId { get; set; }
    }
}

using CodeLinq.Data.Contracts.Interfaces.Entities.Base;

namespace CodeLinq.Data.Contracts.Interfaces.Entities
{
    /// <summary>
    /// Defines a contract that specifies a Category entity object
    /// </summary>
    public interface ICategory : IUniqueIdentifier
    {
        /// <summary>
        /// Gets or sets the friendly name of the category
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the category
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier of the category's parent, or null if a root category.
        /// </summary>
        object ParentCategoryId { get; set; }
    }
}

using CodeLinq.Data.Contracts.Interfaces.Entities.Base;

namespace CodeLinq.Data.Contracts.Interfaces.Entities
{
    /// <summary>
    /// Defines a contract that specifies a Product entity object
    /// </summary>
    public interface IProduct : IUniqueIdentifier
    {
        /// <summary>
        /// Gets or sets the friendly name of the product
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the desription of the product.
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Gets or sets the stock-keeping unit (SKU) is a scannable bar code, most often seen printed on product labels in a retail store.
        /// </summary>
        string Sku { get; set; }
        /// <summary>
        /// Gets or sets the purchase price of the product for display to the customer
        /// </summary>
        decimal Price { get; set; }
        /// <summary>
        /// Gets or sets a value indicating if this product is in stock or not.
        /// </summary>
        bool OutOfStock { get; set; }

    }
}

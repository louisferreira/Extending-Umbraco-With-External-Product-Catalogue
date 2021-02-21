using CodeLinq.Data.Contracts.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLinq.Data.ServicesTests.TestEntities
{
    public class Product : IProduct
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public bool OutOfStock { get; set; }
        public object Id { get; set; }
    }
}

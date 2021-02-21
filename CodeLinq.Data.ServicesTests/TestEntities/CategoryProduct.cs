using CodeLinq.Data.Contracts.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLinq.Data.ServicesTests.TestEntities
{
    public class CategoryProduct : ICategoryProduct
    {
        public object CategoryId { get; set; }
        public object ProductId { get; set; }
        public object Id { get; set; }
    }
}

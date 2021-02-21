using CodeLinq.Data.Contracts.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLinq.Data.ServicesTests.TestEntities
{
    public class Category : ICategory
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public object ParentCategoryId { get; set; }
        public object Id { get; set; }
    }
}

using CodeLinq.Data.Contracts.Infrastructure;
using CodeLinq.Data.Contracts.Interfaces.Entities;
using CodeLinq.Data.Contracts.Interfaces.Infrastruture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLinq.Data.Contracts.Models
{
    public class Media : IMedia
    {
        public object EntityId { get; set; }
        public MediaType MediaType { get; set; }
        public EntityType EntityType { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string Name { get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
        public object Id { get; set; }
    }
}

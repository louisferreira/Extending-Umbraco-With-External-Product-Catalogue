using CodeLinq.Data.Contracts.Interfaces.Entities;
using CodeLinq.Data.Contracts.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLinq.Data.Contracts.Models
{
    public class OperationResult<TEntity> : IOperationResult<TEntity>
    {
        public OperationOutcome OperationOutcome { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public TEntity Entity { get; set; }
    }
}

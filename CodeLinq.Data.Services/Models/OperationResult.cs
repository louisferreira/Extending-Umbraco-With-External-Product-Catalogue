using CodeLinq.Data.Contracts.Interfaces.Infrastructure;

namespace CodeLinq.Data.Services.Models
{
    public class OperationResult<TEntity> : IOperationResult<TEntity>
    {
        public OperationOutcome OperationOutcome { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public TEntity Entity { get; set; }
    }
}

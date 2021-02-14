namespace CodeLinq.Data.Contracts.Interfaces.Infrastructure
{
    /// <summary>
    /// Result value indicating the outcome of an operation.
    /// </summary>
    public enum OperationOutcome
    {
        Success = 0,
        InternalError = 1,
        DatastoreError = 2,
        NotFound = 3,
        UnAuthorised = 4
    }

    /// <summary>
    /// An object representing the outcome details of an operation.
    /// </summary>
    public interface IOperationResult<TEntity>
    {
        /// <summary>
        /// A Enum value indicating the result of the repository operation
        /// </summary>
        OperationOutcome OperationOutcome { get; set; }
        /// <summary>
        /// A number referring to the operation code resulting from the repository operation (0 = success)
        /// </summary>
        int ResultCode { get; set; }
        /// <summary>
        /// The message from the repository resulting from the operation
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// The resulting Entity after succesful operation of the repository, of null if deleted.
        /// </summary>
        TEntity Entity { get; set; }
    }
}

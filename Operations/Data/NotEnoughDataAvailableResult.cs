using IRIS.Operations.Abstract;

namespace IRIS.Operations.Data
{
    /// <summary>
    /// Result indicating that not enough data was available to perform the operation.
    /// </summary>
    public readonly struct NotEnoughDataAvailableResult : IDeviceOperationResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;

        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(NotEnoughDataAvailableResult result) => result.IsSuccess; 
    }
}
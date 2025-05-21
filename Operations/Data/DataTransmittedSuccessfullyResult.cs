using IRIS.Operations.Abstract;

namespace IRIS.Operations.Data
{
    /// <summary>
    /// Result indicating that data was successfully transmitted.
    /// </summary>
    public readonly struct DataTransmittedSuccessfullyResult : IDeviceOperationResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => true;

        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DataTransmittedSuccessfullyResult result) => result.IsSuccess; 
    }
}
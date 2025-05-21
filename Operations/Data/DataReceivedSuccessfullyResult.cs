using IRIS.Operations.Abstract;

namespace IRIS.Operations.Data
{
    /// <summary>
    /// Result indicating that data was successfully received, with the received data included.
    /// </summary>
    /// <typeparam name="TData">Type of data included in the result.</typeparam>
    public readonly struct DataReceivedSuccessfullyResult<TData>(TData data) : IDeviceOperationResult<TData>
    {
        /// <summary>
        /// Data included in the result.
        /// </summary>
        public TData? Data { get; } = data;

        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => true;

        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DataReceivedSuccessfullyResult<TData> result) => result.IsSuccess;
    }
}
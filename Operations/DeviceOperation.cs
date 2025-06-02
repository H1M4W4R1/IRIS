using IRIS.Operations.Abstract;

namespace IRIS.Operations
{
    /// <summary>
    ///     Provides utility methods for device operation results.
    /// </summary>
    public static class DeviceOperation
    {
        /// <summary>
        ///     Determines if the operation result indicates success.
        /// </summary>
        public static bool IsSuccess(IDeviceOperationResult result) => result.IsSuccess;

        /// <summary>
        ///     Determines if the operation result indicates failure.
        /// </summary>
        public static bool IsFailure(IDeviceOperationResult result) => !result.IsSuccess;

        /// <summary>
        ///     Determines if the operation result indicates success and provides a proxy.
        /// </summary>
        public static bool IsSuccess(IDeviceOperationResult result, out IDeviceOperationResult proxy)
        {
            proxy = result;
            return IsSuccess(result);
        }

        /// <summary>
        ///     Determines if the operation result indicates failure and provides a proxy.
        /// </summary>
        public static bool IsFailure(IDeviceOperationResult result, out IDeviceOperationResult proxy)
        {
            proxy = result;
            return IsFailure(result);
        }

        /// <summary>
        ///     Returns a value task with the operation result.
        /// </summary>
        public static ValueTask<IDeviceOperationResult> VResult<TOperationResult>()
            where TOperationResult : struct, IDeviceOperationResult =>
            ValueTask.FromResult(Result<TOperationResult>());

        /// <summary>
        ///     Returns a new instance of the operation result.
        /// </summary>
        public static IDeviceOperationResult Result<TOperationResult>()
            where TOperationResult : struct, IDeviceOperationResult => new TOperationResult();

        /// <summary>
        ///     Returns the data included in the operation result.
        /// </summary>
        /// <param name="result">Device operation result.</param>
        /// <typeparam name="TData">Type of data included in the result.</typeparam>
        /// <returns>Data included in the operation result or null if the operation result is not of the expected type.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the operation result is not of the expected type.</exception>
        public static TData GetData<TData>(IDeviceOperationResult result)
            where TData : notnull
        {
            // Check if operation is of valid type
            if (result is IDeviceOperationResult<TData> operationResult) return operationResult.Data;

#if DEBUG
            throw new InvalidOperationException("Operation result is not of the expected type.");
#else
            return default!;
#endif
        }
    }
}
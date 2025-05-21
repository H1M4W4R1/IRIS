using IRIS.Operations.Abstract;

namespace IRIS.Operations.Generic
{
    /// <summary>
    ///     Represents a result when a device is not ready for operations.
    /// </summary>
    public readonly struct DeviceFeatureNotSupportedResult(bool isSuccess) : IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => isSuccess;

        /// <summary>
        ///     Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceFeatureNotSupportedResult result) => result.IsSuccess;
    }
}

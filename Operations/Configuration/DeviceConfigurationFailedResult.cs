using IRIS.Operations.Abstract;

namespace IRIS.Operations.Configuration
{
    /// <summary>
    ///     Represents a result when a device is not properly configured for operations
    ///     due to a configuration failure.
    /// </summary>
    public readonly struct DeviceConfigurationFailedResult : IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;

        /// <summary>
        ///     Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceConfigurationFailedResult result) => result.IsSuccess;
    }
}

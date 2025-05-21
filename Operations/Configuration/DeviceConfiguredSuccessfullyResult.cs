using IRIS.Operations.Abstract;

namespace IRIS.Operations.Configuration
{
    /// <summary>
    ///     Represents a result when a device has been successfully configured.
    /// </summary>
    public readonly struct DeviceConfiguredSuccessfullyResult : IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => true;

        /// <summary>
        ///     Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceConfiguredSuccessfullyResult result) => result.IsSuccess;
    }
}

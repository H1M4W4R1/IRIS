using IRIS.Operations.Abstract;

namespace IRIS.Operations.Generic
{
    /// <summary>
    /// Represents a result when a device is not available.
    /// </summary>
    public readonly struct DeviceNotAvailableResult : IDeviceOperationResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;
        
        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceNotAvailableResult result) => result.IsSuccess;
    }
}

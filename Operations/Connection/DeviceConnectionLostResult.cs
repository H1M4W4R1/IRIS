using IRIS.Operations.Abstract;
using IRIS.Operations.Generic;

namespace IRIS.Operations.Connection
{
    /// <summary>
    /// Represents a result when an established connection to a device has been unexpectedly lost.
    /// </summary>
    public readonly struct DeviceConnectionLostResult : IDeviceOperationResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;
        
        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceConnectionLostResult result) => result.IsSuccess;
    }
}

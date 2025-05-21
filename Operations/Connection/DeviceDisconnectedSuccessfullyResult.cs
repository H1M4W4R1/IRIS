using IRIS.Operations.Abstract;
using IRIS.Operations.Generic;

namespace IRIS.Operations.Connection
{
    /// <summary>
    /// Represents a result when a device has been successfully disconnected.
    /// </summary>
    public readonly struct DeviceDisconnectedSuccessfullyResult : IDeviceOperationResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => true;
        
        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceDisconnectedSuccessfullyResult result) => result.IsSuccess;
    }
}

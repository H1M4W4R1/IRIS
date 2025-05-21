using IRIS.Operations.Abstract;
using IRIS.Operations.Generic;

namespace IRIS.Operations.Connection
{
    /// <summary>
    /// Represents a result when an attempt to disconnect from a device has succeeded,
    /// but we can't disconnect due to other devices using same connection / interface.
    /// </summary>
    public readonly struct DeviceAmbiguousDisconnectionResult : IDeviceOperationResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;
        
        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceAmbiguousDisconnectionResult result) => result.IsSuccess;
    }
}

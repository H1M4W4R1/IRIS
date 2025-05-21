using IRIS.Operations.Abstract;
using IRIS.Operations.Generic;

namespace IRIS.Operations.Connection
{
    /// <summary>
    /// Represents a result when a device could not be found.
    /// </summary>
    public readonly struct DeviceNotFoundResult : IDeviceOperationResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;
        
        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceNotFoundResult result) => result.IsSuccess;
    }
}
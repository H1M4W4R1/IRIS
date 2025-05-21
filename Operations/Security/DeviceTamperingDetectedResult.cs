using IRIS.Operations.Abstract;
using IRIS.Operations.Generic;

namespace IRIS.Operations.Security
{
    /// <summary>
    /// Represents a result when tampering or unauthorized modification of a device is detected.
    /// </summary>
    public readonly struct DeviceTamperingDetectedResult : IDeviceOperationResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;
        
        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceTamperingDetectedResult result) => result.IsSuccess;
    }
}

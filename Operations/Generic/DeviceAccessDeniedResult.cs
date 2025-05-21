using IRIS.Operations.Abstract;

namespace IRIS.Operations.Generic
{
    /// <summary>
    /// Represents a result when access to a device is denied due to permissions or other restrictions.
    /// </summary>
    public readonly struct DeviceAccessDeniedResult : IDeviceOperationResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;
        
        /// <summary>
        /// Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceAccessDeniedResult result) => result.IsSuccess;
    }
}

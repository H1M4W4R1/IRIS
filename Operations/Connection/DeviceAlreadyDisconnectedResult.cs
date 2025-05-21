using IRIS.Operations.Abstract;

namespace IRIS.Operations.Connection
{
    /// <summary>
    ///     Represents a result when a device is already disconnected.
    /// </summary>
    public readonly struct DeviceAlreadyDisconnectedResult : IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => true;

        /// <summary>
        ///     Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceAlreadyDisconnectedResult result) => result.IsSuccess; 
    }
}
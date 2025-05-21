using IRIS.Operations.Abstract;

namespace IRIS.Operations.Connection
{
    /// <summary>
    ///     Represents a result when an attempt to disconnect from a device has failed.
    /// </summary>
    public readonly struct DeviceDisconnectionFailedResult : IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;

        /// <summary>
        ///     Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceDisconnectionFailedResult result) => result.IsSuccess;
    }
}

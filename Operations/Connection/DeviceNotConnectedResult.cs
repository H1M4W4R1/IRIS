using IRIS.Operations.Abstract;

namespace IRIS.Operations.Connection
{
    /// <summary>
    ///     Represents a result when an operation is attempted on a device that is not currently connected.
    /// </summary>
    public readonly struct DeviceNotConnectedResult : IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;

        /// <summary>
        ///     Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceNotConnectedResult result) => result.IsSuccess;
    }
}

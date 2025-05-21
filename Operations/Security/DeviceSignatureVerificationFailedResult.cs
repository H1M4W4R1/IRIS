using IRIS.Operations.Abstract;

namespace IRIS.Operations.Security
{
    /// <summary>
    ///     Represents a result when digital signature verification with a device fails.
    /// </summary>
    public readonly struct DeviceSignatureVerificationFailedResult : IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;

        /// <summary>
        ///     Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceSignatureVerificationFailedResult result) => result.IsSuccess;
    }
}

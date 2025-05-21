using IRIS.Operations.Abstract;

namespace IRIS.Operations.Data
{
    /// <summary>
    ///     Result indicating that a notification un-subscription failed.
    /// </summary>
    public readonly struct DeviceUnsubscriptionFailedResult : IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => false;

        /// <summary>
        ///     Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceUnsubscriptionFailedResult result) => result.IsSuccess; 
    }
}
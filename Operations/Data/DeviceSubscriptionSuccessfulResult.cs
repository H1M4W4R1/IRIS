using IRIS.Operations.Abstract;

namespace IRIS.Operations.Data
{
    /// <summary>
    ///     Result indicating that a notification subscription was successfully performed.
    /// </summary>
    public readonly struct DeviceSubscriptionSuccessfulResult : IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => true;

        /// <summary>
        ///     Implicitly converts the result to a boolean indicating success.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        public static implicit operator bool(DeviceSubscriptionSuccessfulResult result) => result.IsSuccess; 
    }
}
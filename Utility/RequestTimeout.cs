using System.Runtime.CompilerServices;

namespace IRIS.Utility
{
    /// <summary>
    ///     Represents a timeout configuration for asynchronous operations.
    /// </summary>
    /// <param name="milliseconds">The duration in milliseconds after which the timeout will trigger.</param>
    /// <remarks>
    ///     When initialized with a value of 0 or less, no timeout will be enforced as the default cancellation token will be
    ///     used.
    ///     This struct provides a convenient way to handle timeouts in async operations through implicit conversion to
    ///     CancellationToken.
    /// </remarks>
    public readonly struct RequestTimeout(int milliseconds)
    {
        /// <summary>
        ///     The underlying CancellationTokenSource that manages the timeout.
        /// </summary>
        /// <remarks>
        ///     This property is null when milliseconds is 0 or less, indicating no timeout should be enforced.
        /// </remarks>
        private CancellationTokenSource? TokenSource { get; } = 
            milliseconds <= 0 ? null : new CancellationTokenSource(milliseconds);

        /// <summary>
        ///     Gets the CancellationToken associated with this timeout.
        /// </summary>
        /// <remarks>
        ///     Returns the default CancellationToken if no timeout is configured (TokenSource is null).
        /// </remarks>
        private CancellationToken Token => TokenSource?.Token ?? default;

        /// <summary>
        ///     Indicates whether the timeout period has elapsed.
        /// </summary>
        /// <remarks>
        ///     Returns true if the associated CancellationToken has been cancelled due to timeout.
        /// </remarks>
        public bool IsTimedOut => Token.IsCancellationRequested;

        /// <summary>
        ///     Implicitly converts a RequestTimeout to a CancellationToken.
        /// </summary>
        /// <param name="timeout">The RequestTimeout instance to convert.</param>
        /// <returns>The CancellationToken associated with the timeout.</returns>
        /// <remarks>
        ///     This conversion allows RequestTimeout to be used directly in async methods that accept CancellationToken.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CancellationToken(RequestTimeout timeout) => timeout.Token;

        /// <summary>
        ///     Implicitly converts an integer value to a RequestTimeout.
        /// </summary>
        /// <param name="milliseconds">The timeout duration in milliseconds.</param>
        /// <returns>A new RequestTimeout instance configured with the specified duration.</returns>
        /// <remarks>
        ///     This conversion allows integer values to be used directly where a RequestTimeout is expected.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RequestTimeout(int milliseconds) => new(milliseconds);
    }
}
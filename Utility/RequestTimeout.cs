using System.Runtime.CompilerServices;

namespace IRIS.Utility
{
    /// <summary>
    /// Represents a timeout in milliseconds.
    /// </summary>
    /// <param name="milliseconds">Time when the timeout will occur in milliseconds.</param>
    /// <remarks>
    /// If the timeout is set to 0 or less, no timeout will occur as default token will be used.
    /// </remarks>
    public readonly struct RequestTimeout(int milliseconds)
    {
        /// <summary>
        /// Token source for the timeout.
        /// </summary>
        private CancellationTokenSource? TokenSource { get; } = 
            milliseconds <= 0 ? null : new CancellationTokenSource(milliseconds);
        
        /// <summary>
        /// Gets the token for the timeout.
        /// </summary>
        private CancellationToken Token => TokenSource?.Token ?? default;

        /// <summary>
        /// Checks if the timeout has occurred.
        /// </summary>
        public bool IsTimedOut => Token.IsCancellationRequested;

        /// <summary>
        /// Converts the timeout to a token.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CancellationToken(RequestTimeout timeout) => timeout.Token;
        
        /// <summary>
        /// Converts the time in milliseconds to a timeout.
        /// </summary>
        /// <param name="milliseconds">Time in milliseconds.</param>
        /// <returns>Converted timeout.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RequestTimeout(int milliseconds) => new(milliseconds);
    }
}
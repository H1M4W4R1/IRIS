using System.Runtime.CompilerServices;

namespace IRIS.Utility.Awaitable
{
    /// <summary>
    ///     Provides an awaitable implementation for waiting until a collection is at least a certain length.
    /// </summary>
    /// <remarks>
    ///     This class implements the INotifyCompletion interface to support the await pattern.
    ///     It allows asynchronous code to wait for an array to be at least a certain length.
    ///     The awaiter can be cancelled using a CancellationToken.
    /// </remarks>
    public sealed class CollectionLengthAwaiter<TArrayType>(IEnumerable<TArrayType> enumerable,
        int desiredLength,
        CancellationToken cancellationToken = default
    ) : INotifyCompletion
    {
        private bool _isCompleted = false;
        private Action _continuation = () => { };

        /// <summary>
        /// Checks if awaiter was completed
        /// </summary>
        public bool IsCompleted => _isCompleted;

        /// <summary>
        /// Gets the result
        /// </summary>
        /// <returns>Array length</returns>
        public int GetResult() => _GetEnumerableLength();

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;

            // ReSharper disable once MethodSupportsCancellation
            // We don't need cancellation as it will be handled within the loop
            Task.Run(InternalLoop);
        }

        private ValueTask InternalLoop()
        {
            // Wait until array length is greater than or equal to desired length
            while (_GetEnumerableLength() < desiredLength)
            {
                if (!cancellationToken.IsCancellationRequested) continue;
                _isCompleted = true;
                _continuation();
                return ValueTask.CompletedTask;
            }

            _isCompleted = true;
            _continuation();
            return ValueTask.CompletedTask;
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int _GetEnumerableLength()
        {
            lock (enumerable)
            {
                if (enumerable is TArrayType[] array) return array.Length;
                if (enumerable is ICollection<TArrayType> collection) return collection.Count;
                return enumerable.Count();
            }
        }
    }
}
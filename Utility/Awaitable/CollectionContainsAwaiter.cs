using System.Runtime.CompilerServices;

namespace IRIS.Utility.Awaitable
{
    /// <summary>
    ///     Provides an awaitable implementation for waiting until a collection contains a desired object.
    /// </summary>
    /// <remarks>
    ///     This class implements the INotifyCompletion interface to support the await pattern.
    ///     It allows asynchronous code to wait for until an array contains a desired object
    ///     before proceeding. The awaiter can be cancelled using a CancellationToken.
    /// </remarks>
    public sealed class CollectionContainsAwaiter<TArrayType>(IEnumerable<TArrayType> enumerable,
        TArrayType desiredObject,
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
        /// <returns>True if array contains desired object</returns>
        public bool GetResult() => _Contains(); 
        
        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;

            // ReSharper disable once MethodSupportsCancellation
            // We don't need cancellation as it will be handled within the loop
            Task.Run(InternalLoop);
        }

        private ValueTask InternalLoop()
        {
            // Wait until array contains desired object
            while (!_Contains())
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

        private bool _Contains()
        {
            lock (enumerable)
            {
                if (enumerable is TArrayType[] array)
                {
                    for (int i = array.Length - 1; i >= 0; i--)
                    {
                        // Move iterator if array was resized
                        if (i >= array.Length) continue;

                        TArrayType arrayObject = array[i];

                        // Check if element is null
                        if (arrayObject == null) return desiredObject == null;

                        // Check if element equals desired object
                        if (arrayObject.Equals(desiredObject)) return true;
                    }

                    return false;
                }

                if (enumerable is ICollection<TArrayType> collection)
                {
                    for (int i = collection.Count - 1; i >= 0; i--)
                    {
                        // Move iterator if array was resized
                        if (i >= collection.Count) continue;

                        TArrayType collectionObject = collection.ElementAt(i);

                        // Check if element is null
                        if (collectionObject == null) return desiredObject == null;

                        // Check if element equals desired object
                        if (collectionObject.Equals(desiredObject)) return true;
                    }

                    return false;
                }

                return enumerable.Contains(desiredObject); // IEnumerable()
            }
        }
    }
}
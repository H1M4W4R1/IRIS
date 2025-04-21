namespace IRIS.Utility
{
    public static class Extensions
    {
        /// <summary>
        /// Wait for the task to complete
        /// </summary>
        /// <param name="valueTask">The ValueTask to wait for</param>
        /// <param name="cancellationToken">The cancellation token to cancel the wait</param>
        /// <exception cref="OperationCanceledException">Thrown when the wait is cancelled</exception>
        public static void Wait(this ValueTask valueTask, CancellationToken cancellationToken = default)
        {
            // Wait for the task to complete
            while(!valueTask.IsCompleted) cancellationToken.ThrowIfCancellationRequested();
        }
        
        /// <summary>
        /// Wait for the task to complete and return the result
        /// </summary>
        /// <param name="valueTask">The ValueTask to wait for</param>
        /// <param name="cancellationToken">The cancellation token to cancel the wait</param>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <returns>The result of the ValueTask</returns>
        public static TResult Wait<TResult>(this ValueTask<TResult> valueTask, CancellationToken cancellationToken = default)
        {
            // Wait for the task to complete
            while(!valueTask.IsCompleted) cancellationToken.ThrowIfCancellationRequested();
            
            // Return the result
            return valueTask.Result;
        }
    }
}
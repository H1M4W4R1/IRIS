using System.Diagnostics;

namespace IRIS.Utility
{
    /// <summary>
    ///     Provides extension methods for ValueTask operations.
    /// </summary>
    public static class ValueTaskExtensions
    {
        /// <summary>
        ///     Executes a ValueTask asynchronously without awaiting its result.
        ///     This method is useful for fire-and-forget scenarios where the task's result is not needed.
        /// </summary>
        /// <param name="valueTask">The ValueTask to execute.</param>
        /// <remarks>
        ///     This method executes the ValueTask and handles any exceptions by logging them.
        ///     Exceptions are logged using Debug.WriteLine to avoid losing error information.
        /// </remarks>
        public static async void Forget(this ValueTask valueTask)
        {
            try
            {
                await valueTask;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unhandled exception in Forget: {ex}");
            }
        }
    }
}
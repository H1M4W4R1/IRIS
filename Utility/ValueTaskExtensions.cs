using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace IRIS.Utility
{
    public static class ValueTaskExtensions
    {
        public static async void Forget(this ValueTask valueTask)
        {
            await valueTask;
        }
    }
}
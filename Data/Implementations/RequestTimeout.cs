namespace IRIS.Data.Implementations
{
    public sealed class RequestTimeout : ErrorResponse
    {
        public static RequestTimeout Instance { get; } = new();
    }
}
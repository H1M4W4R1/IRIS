namespace IRIS.Data.Implementations
{
    /// <summary>
    /// Represents that no response from device was received
    /// </summary>
    public sealed class NoResponse : DeviceResponseBase
    {
        public static NoResponse Instance { get; } = new();
    }
}
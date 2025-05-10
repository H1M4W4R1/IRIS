namespace IRIS.Data.Implementations
{
    /// <summary>
    /// Represents OK response from device
    /// </summary>
    // ReSharper disable once ClassCanBeSealed.Global
    public class OKResponse : DeviceResponseBase
    {
        public static OKResponse Instance { get; } = new();
        
    }
}
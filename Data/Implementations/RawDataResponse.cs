namespace IRIS.Data.Implementations
{
    /// <summary>
    /// Represents raw data response from device
    /// </summary>
    /// <param name="data">Data received from device</param>
    public sealed class RawDataResponse(byte[] data) : DeviceResponseBase<byte[]>(data)
    {
        
    }
}
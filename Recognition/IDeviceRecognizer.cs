using IRIS.Addressing;

namespace IRIS.Recognition
{
    /// <summary>
    /// An interface for device recognizers
    /// Device recognizer is used to scan for devices and check if device is supported - it can be used to
    /// automatically connect to devices or to check if device is supported by the system while connecting to it
    /// An example recognizer can be seen at <see cref="WindowsUSBSerialPortRecognizer"/>
    /// </summary>
    /// <typeparam name="TDeviceAddress">Type of device address for example <see cref="SerialPortDeviceAddress"/></typeparam>
    public interface IDeviceRecognizer<TDeviceAddress> where TDeviceAddress : IDeviceAddress
    {
        /// <summary>
        /// Scan for all available devices
        /// </summary>
        List<TDeviceAddress> ScanForDevices();
    
        /// <summary>
        /// Check if device is supported by this recognizer (if it exists)
        /// </summary>
        /// <param name="deviceAddress">Address of the device</param>
        /// <returns>True if device is supported, false otherwise</returns>
        /// <remarks>
        /// Remember to check if TDeviceAddress is supported by this recognizer (otherwise API will have
        /// unexpected behavior)
        /// </remarks>
        bool CheckDevice(TDeviceAddress deviceAddress);
    }
}

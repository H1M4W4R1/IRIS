using IRIS.Addressing;
using IRIS.Communication.Serial;
using IRIS.Communication.Serial.Settings;

namespace IRIS.Devices
{
    /// <summary>
    /// Represents device connected to computer via serial port
    /// For example COM9 or /dev/ttyUSB0
    /// </summary>
    public abstract class SerialDeviceBase : DeviceBase<CachedSerialPortInterface, SerialPortDeviceAddress>
    {
        /// <summary>
        /// Create serial device with specific address and settings
        /// </summary>
        protected SerialDeviceBase(SerialPortDeviceAddress deviceAddress, SerialInterfaceSettings settings)
        {
            Interface = 
                new CachedSerialPortInterface(deviceAddress.Address, settings.baudRate, settings.parity, settings.dataBits, settings.stopBits);
        }
        
        /// <summary>
        /// Change device address to new one
        /// </summary>
        public void SetAddress(SerialPortDeviceAddress deviceAddress)
        {
            // Check if port is open
            bool wasPortOpen = Interface.IsOpen;
            if (wasPortOpen)
                Interface.Disconnect();

            Interface.PortName = deviceAddress.ToString();

            // If port was open then connect again
            if (wasPortOpen)
                Interface.Connect();
        }
    }
}
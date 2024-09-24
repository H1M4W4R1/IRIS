using IRIS.Addressing;
using IRIS.Devices.Interfaces;
using IRIS.Devices.Interfaces.Settings;

namespace IRIS.Devices
{
    /// <summary>
    /// Represents device connected to computer via serial port
    /// For example COM9 or /dev/ttyUSB0
    /// </summary>
    public abstract class SerialDeviceBase : DeviceBase<SerialPortInterface, SerialPortDeviceAddress>
    {
        /// <summary>
        /// Create serial device with specific address and settings
        /// </summary>
        protected SerialDeviceBase(SerialPortDeviceAddress deviceAddress, SerialInterfaceSettings settings)
        {
            Interface = 
                new SerialPortInterface(deviceAddress.Address, settings.baudRate, settings.parity, settings.dataBits, settings.stopBits);
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
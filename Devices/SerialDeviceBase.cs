using IRIS.Addressing;
using IRIS.Devices.Interfaces;
using IRIS.Devices.Interfaces.Settings;
using IRIS.Protocols;

namespace IRIS.Devices
{
    /// <summary>
    /// Represents device connected to computer via serial port
    /// For example COM9 or /dev/ttyUSB0
    /// </summary>
    /// <typeparam name="TProtocol">Protocol used for data exchange</typeparam>
    public abstract class SerialDeviceBase<TProtocol> : DeviceBase<SerialPortInterface, TProtocol, SerialPortDeviceAddress> 
        where TProtocol : struct, IProtocol
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
        /// Send command to device
        /// </summary>
        public void SendData<TData>(TData data) where TData : unmanaged
        {
            byte[] encodedData = CommunicationProtocol.EncodeData(data);
            Interface.TransmitData(encodedData);
        }
        
        /// <summary>
        /// Receive data from device
        /// </summary>
        public TData ReceiveData<TData>() where TData : unmanaged
        {
            // Try to read data from device, if it fails return default value, otherwise decode data and return object
            return CommunicationProtocol.TryToReadData(Interface, out TData data) ? data : default;
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
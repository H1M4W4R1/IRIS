using IRIS.Communication.Protocols;
using IRIS.Communication.Protocols.Addressing;
using IRIS.Communication.Protocols.Implementations.Abstractions;
using IRIS.Communication.Protocols.Recognition;

namespace IRIS.Devices
{
    public interface IDevice<TSelf> : IDevice where TSelf : IDevice<TSelf>
    {

        /// <summary>
        /// Build and connect to device
        /// </summary>
        public TSelf BuildAndConnect()
        {
            return Build().Connect();
        }

        /// <summary>
        /// Build this device infrastructure
        /// </summary>
        TSelf Build();

        /// <summary>
        /// Connect to device
        /// </summary>
        TSelf Connect();

        /// <summary>
        /// Disconnect from device
        /// </summary>
        TSelf Disconnect();

        /// <summary>
        /// Change device address
        /// </summary>
        TSelf SetAddress(DeviceAddress address);       

        /// <summary>
        /// Begin building this device
        /// Use this method to implement constant parameters of device (like <see cref="Communication.Protocols.IProtocol{TSelf}"/>)
        /// </summary>
        public static abstract TSelf Create();

        /// <summary>
        /// Validate this device, should throw exception on invalid configuration
        /// </summary>
        TSelf Validate();

        /// <summary>
        /// Execute device command
        /// </summary>
        T Execute<T>(InvokableCommand<T> command) where T : IReceivedDataObject<T>;

        /// <summary>
        /// Seek for devices of this type, can throw <see cref="NotImplementedException"/>
        /// </summary>
        public List<DeviceAddress> SeekForDevices();

        /// <summary>
        /// Implement device recognition
        /// </summary>
        TSelf UsesRecognition(IDeviceRecognizer recognizer);
    }

    public interface IDevice
    {
        IProtocol? GetCurrentProtocol();

        /// <summary>
        /// Get device recognizer
        /// </summary>
        IDeviceRecognizer? GetDeviceRecognizer();
    }
}

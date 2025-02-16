using IRIS.Communication.Types;

namespace IRIS.Communication
{
    public interface ICommunicationInterface<out TAddressType> : ICommunicationInterface
    {
        /// <summary>
        /// Event that is triggered when device is connected
        /// </summary>
        public event Delegates.OnDeviceConnected<TAddressType>? DeviceConnected;
        
        /// <summary>
        /// Event that is triggered when device is disconnected
        /// </summary>
        public event Delegates.OnDeviceDisconnected<TAddressType>? DeviceDisconnected;
        
        /// <summary>
        /// Event that is triggered when device connection is lost
        /// </summary>
        public event Delegates.DeviceConnectionLost<TAddressType>? DeviceConnectionLost;
    }
    
    /// <summary>
    /// Represents communication interface between device and computer
    /// This can be for example serial port, ethernet, etc.
    /// </summary>
    public interface ICommunicationInterface
    {
        /// <summary>
        /// Connect to physical device
        /// </summary>
        ValueTask<bool> Connect(CancellationToken cancellationToken = default);

        /// <summary>
        /// Disconnect from physical device
        /// </summary>
        ValueTask<bool> Disconnect(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get raw data communication interface if available
        /// </summary>
        public IRawDataCommunicationInterface? Raw => this as IRawDataCommunicationInterface;
    }
}
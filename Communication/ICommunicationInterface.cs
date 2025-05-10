namespace IRIS.Communication
{
    /// <summary>
    /// Represents communication interface between device and computer
    /// This can be for example serial port, ethernet, etc.
    /// </summary>
    public interface ICommunicationInterface<out TAddressType> : ICommunicationInterface
    {
        /// <summary>
        /// Event that is triggered when device is connected
        /// </summary>
        public event Delegates.DeviceConnectedHandler<TAddressType>? DeviceConnected;
        
        /// <summary>
        /// Event that is triggered when device is disconnected
        /// </summary>
        public event Delegates.DeviceDisconnectedHandler<TAddressType>? DeviceDisconnected;
        
        /// <summary>
        /// Event that is triggered when device connection is lost
        /// </summary>
        public event Delegates.DeviceConnectionLostHandler<TAddressType>? DeviceConnectionLost;
    }
    
    /// <summary>
    /// <see cref="ICommunicationInterface{TAddressType}"/>
    /// </summary>
    public interface ICommunicationInterface
    {
        /// <summary>
        /// Connect to physical device
        /// </summary>
        bool Connect(CancellationToken cancellationToken = default);

        /// <summary>
        /// Disconnect from physical device
        /// </summary>
        bool Disconnect();
    }
}
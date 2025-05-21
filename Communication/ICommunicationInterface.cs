using IRIS.Operations.Abstract;

namespace IRIS.Communication
{
    /// <summary>
    ///     Represents a communication interface between a device and computer system.
    ///     This interface provides the foundation for various communication protocols
    ///     such as serial ports, Ethernet, USB, or other physical/virtual connections.
    /// </summary>
    /// <typeparam name="TAddressType">The type of address used to identify the communication endpoint.</typeparam>
    public interface ICommunicationInterface<out TAddressType> : ICommunicationInterface
    {
        /// <summary>
        ///     Event that is triggered when a device successfully establishes a connection.
        ///     This event provides the address of the newly connected device.
        /// </summary>
        public event Delegates.DeviceConnectedHandler<TAddressType>? DeviceConnected;

        /// <summary>
        ///     Event that is triggered when a device is properly disconnected.
        ///     This event provides the address of the disconnected device.
        /// </summary>
        public event Delegates.DeviceDisconnectedHandler<TAddressType>? DeviceDisconnected;

        /// <summary>
        ///     Event that is triggered when a device connection is unexpectedly lost.
        ///     This event provides the address of the device that lost connection.
        /// </summary>
        public event Delegates.DeviceConnectionLostHandler<TAddressType>? DeviceConnectionLost;
    }

    /// <summary>
    ///     Base interface for device communication that defines core connection management methods.
    ///     This interface serves as the foundation for all communication interfaces in the system.
    /// </summary>
    /// <seealso cref="ICommunicationInterface{TAddressType}" />
    public interface ICommunicationInterface
    {
        /// <summary>
        ///     Establishes a connection to the physical device.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the connection attempt.</param>
        ValueTask<IDeviceOperationResult> Connect(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Terminates the connection to the physical device.
        /// </summary>
        ValueTask<IDeviceOperationResult> Disconnect();
    }
}
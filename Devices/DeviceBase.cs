using IRIS.Addressing.Abstract;
using IRIS.Communication;
using IRIS.Operations.Abstract;

namespace IRIS.Devices
{
    /// <summary>
    ///     Base class for devices that are connected to a computer system.
    ///     This abstract class provides the foundation for device communication and management.
    /// </summary>
    /// <typeparam name="TCommunicationInterface">The type of communication interface used to interact with the device.</typeparam>
    /// <typeparam name="TAddressType">The type of address used to identify the device on the network.</typeparam>
    public abstract class DeviceBase<TCommunicationInterface, TAddressType> : DeviceBase<TCommunicationInterface>
        where TCommunicationInterface : ICommunicationInterface
        where TAddressType : struct, IDeviceAddress
    {
    }

    /// <summary>
    ///     Base class that provides core functionality for device communication and management.
    ///     This abstract class implements the fundamental operations for connecting to and
    ///     interacting with physical devices through a communication interface.
    /// </summary>
    /// <typeparam name="TCommunicationInterface">The type of communication interface used to interact with the device.</typeparam>
    public abstract class DeviceBase<TCommunicationInterface>
        where TCommunicationInterface : ICommunicationInterface
    {
        /// <summary>
        ///     Establishes a connection to the physical device using the configured communication interface.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the connection attempt.</param>
        public virtual ValueTask<IDeviceOperationResult> Connect(CancellationToken cancellationToken = default)
            => HardwareAccess.Connect(cancellationToken);

        /// <summary>
        ///     Terminates the connection to the physical device using the configured communication interface.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the disconnection attempt.</param>
        public virtual ValueTask<IDeviceOperationResult> Disconnect(CancellationToken cancellationToken = default)
            => HardwareAccess.Disconnect();

        /// <summary>
        ///     Gets the communication interface used to interact with the physical device.
        ///     This property must be initialized in the constructor of derived classes as it is not
        ///     initialized in the base class constructor.
        /// </summary>
        /// <remarks>
        ///     This property is not initialized in the base constructor as the specific communication
        ///     interface implementation is not known at that point. Derived classes must provide
        ///     initialization through their constructors.
        /// </remarks>
        // ReSharper disable once NullableWarningSuppressionIsUsed
        protected TCommunicationInterface HardwareAccess { get; init; } = default!;

        /// <summary>
        ///     Provides access to the hardware communication interface.
        ///     This method should only be used within transaction contexts to ensure proper
        ///     synchronization and resource management.
        /// </summary>
        /// <returns>The communication interface instance used to interact with the device.</returns>
        public TCommunicationInterface AccessHardwareInterface() => HardwareAccess;
    }
}
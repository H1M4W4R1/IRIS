using IRIS.Addressing.Abstract;
using IRIS.Communication;
using IRIS.Communication.Types;

namespace IRIS.Devices
{
    /// <summary>
    /// Represents device connected to computer
    /// </summary>
    /// <typeparam name="TCommunicationInterface">Communication interface between device and computer</typeparam>
    /// <typeparam name="TAddressType">Type of device address</typeparam>
    public abstract class DeviceBase<TCommunicationInterface, TAddressType> : DeviceBase<TCommunicationInterface>
        where TCommunicationInterface : ICommunicationInterface
        where TAddressType : struct, IDeviceAddress
    {
    }

    public abstract class DeviceBase<TCommunicationInterface>
        where TCommunicationInterface : ICommunicationInterface
    {
        /// <summary>
        /// Connect to device
        /// </summary>
        public virtual ValueTask<bool> Connect(CancellationToken cancellationToken = default)
            => HardwareAccess.Connect(cancellationToken);

        /// <summary>
        /// Disconnect from device
        /// </summary>
        public virtual ValueTask<bool> Disconnect(CancellationToken cancellationToken = default)
            => HardwareAccess.Disconnect(cancellationToken);

        /// <summary>
        /// Communication interface between device and computer
        /// <b>Beware: this is not initialized in constructor, as it is not known at this point
        /// You must have a constructor in derived class that initializes this property</b>
        /// </summary>
        protected TCommunicationInterface HardwareAccess { get; init; } = default!;

        /// <summary>
        /// Represents communication interface between device and computer
        /// </summary>
        protected IRawDataCommunicationInterface RawHardwareAccess
        {
            get
            {
                // Check if communication interface is raw data communication interface,
                // if not throw exception to prevent misuse
                if (HardwareAccess is IRawDataCommunicationInterface raw) return raw;
                throw new NotSupportedException("Communication interface does not support raw data access");
            }
        }

        /// <summary>
        /// Used to get communication interface, should be only implemented in transactions
        /// </summary>
        public TCommunicationInterface AccessHardwareInterface() => HardwareAccess;
    }
}
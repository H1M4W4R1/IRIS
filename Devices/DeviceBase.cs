using IRIS.Addressing;
using IRIS.Communication;
using IRIS.Protocols;

namespace IRIS.Devices
{
    /// <summary>
    /// Represents device connected to computer
    /// </summary>
    /// <typeparam name="TProtocol">Protocol used for communication</typeparam>
    /// <typeparam name="TCommunicationInterface">Communication interface between device and computer</typeparam>
    /// <typeparam name="TAddressType">Type of device address</typeparam>
    public abstract class DeviceBase<TCommunicationInterface, TProtocol, TAddressType>
        where TProtocol : struct, IProtocol
        where TCommunicationInterface : ICommunicationInterface
        where TAddressType : struct, IDeviceAddress
    {
        /// <summary>
        /// Communication interface between device and computer
        /// Beware: this is not initialized in constructor, as it is not known at this point
        /// You must have a constructor in derived class that initializes this property
        /// </summary>
        protected TCommunicationInterface Interface { get; init; } = default!;

        /// <summary>
        /// Communication interface between device and computer
        /// </summary>
        protected TProtocol CommunicationProtocol { get; init; } = new TProtocol();
        
        
    }
}
namespace IRIS.Addressing.Abstract
{
    /// <summary>
    /// Represents an abstraction of a device address that can store different types of address values.
    /// For example, this could represent a COM port name, IP address, or other device identifiers.
    /// </summary>
    /// <typeparam name="TAddressStorageType">The type used to store the device address value.</typeparam>
    public interface IDeviceAddress<out TAddressStorageType> : IDeviceAddress
        where TAddressStorageType : notnull
    {
        /// <summary>
        /// Gets the address value of the device.
        /// The actual format depends on the implementation and TAddressStorageType.
        /// Examples include: COM port names, IP addresses, or custom device identifiers.
        /// </summary>
        public TAddressStorageType Address { get; }
    }

    /// <summary>
    /// Base interface for device addressing without type constraints.
    /// Used as a common interface for all device address implementations.
    /// </summary>
    public interface IDeviceAddress
    {

    }
}
namespace IRIS.Addressing
{
    /// <summary>
    /// Represents abstraction of device address - eg. COM Port Name
    /// Used to store device address
    /// </summary>
    public interface IDeviceAddress<out TAddressStorageType> : IDeviceAddress
        where TAddressStorageType : notnull
    {
        /// <summary>
        /// Address of device (such as COM port name)
        /// </summary>
        public TAddressStorageType Address { get; }
    
        /// <summary>
        /// Get address of device
        /// </summary>
        /// <typeparam name="TAddressStorageTypeLow">The type to which the address is to be cast.</typeparam>
        /// <returns>The address of the device cast to the specified type.</returns>
        /// <exception cref="InvalidCastException">Thrown when the address cannot be cast to the specified type.</exception>
        TAddressStorageTypeLow IDeviceAddress.GetAddress<TAddressStorageTypeLow>()
        {
            // Check if the Address can be cast to the specified type
            if (Address is TAddressStorageTypeLow lowLevelAddress) return lowLevelAddress;

            // If the Address cannot be cast to the specified type, throw an exception
            throw new InvalidCastException($"Cannot cast {Address.GetType()} to {typeof(TAddressStorageTypeLow)}");
        }
    }

    public interface IDeviceAddress
    {
        /// <summary>
        /// Get address of device
        /// </summary>
        public TAddressStorageType GetAddress<TAddressStorageType>();
    }
}
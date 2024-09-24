namespace IRIS.Addressing
{
    /// <summary>
    /// Represents abstraction of device address - e.g. COM Port Name
    /// </summary>
    public interface IDeviceAddress<out TAddressStorageType> : IDeviceAddress
        where TAddressStorageType : notnull
    {
        /// <summary>
        /// Address of device (such as COM port name)
        /// </summary>
        public TAddressStorageType Address { get; }
    }

    public interface IDeviceAddress
    {

    }
}
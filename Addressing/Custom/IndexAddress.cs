using IRIS.Addressing.Abstract;

namespace IRIS.Addressing.Custom
{
    /// <summary>
    /// Represents a device address using a 32-bit unsigned integer index.
    /// This implementation allows devices to be accessed using a simple numeric index,
    /// where each device is assigned a unique unsigned integer value.
    /// </summary>
    /// <remarks>
    /// The index is stored as a 32-bit unsigned integer (uint), allowing for values
    /// from 0 to 4,294,967,295. This provides a simple and efficient way to reference
    /// devices in systems where a sequential or arbitrary numeric identifier is appropriate.
    /// </remarks>
    public readonly struct IndexAddress(uint index) : IDeviceAddress<uint>
    {
        /// <summary>
        /// Gets the numeric index value used to identify the device.
        /// </summary>
        /// <value>
        /// A 32-bit unsigned integer representing the device's index.
        /// </value>
        public uint Address { get; } = index;

        /// <summary>
        /// Returns a string representation of the device's index.
        /// </summary>
        /// <returns>
        /// A string containing the numeric value of the device's index.
        /// </returns>
        public override string ToString() => Address.ToString();
    }
}
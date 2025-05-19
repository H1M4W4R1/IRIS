using IRIS.Addressing.Abstract;

namespace IRIS.Addressing.Custom
{
    /// <summary>
    /// Represents a device address using a string name identifier.
    /// This implementation allows devices to be accessed using a human-readable name,
    /// where each device is assigned a unique string identifier.
    /// </summary>
    /// <remarks>
    /// The name is stored as a string, allowing for flexible and descriptive device identification.
    /// This provides a user-friendly way to reference devices in systems where meaningful
    /// names are preferred over numeric or technical identifiers.
    /// </remarks>
    public readonly struct NameAddress(string name) : IDeviceAddress<string>
    {
        /// <summary>
        /// Gets the string name value used to identify the device.
        /// </summary>
        /// <value>
        /// A string representing the device's name identifier.
        /// </value>
        public string Address { get; } = name;
        
        /// <summary>
        /// Returns a string representation of the device's name.
        /// </summary>
        /// <returns>
        /// A string containing the name value of the device.
        /// </returns>
        public override string ToString() => Address;
    }
}
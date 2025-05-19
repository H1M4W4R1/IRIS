namespace IRIS.Protocols.IRIS.Data
{
    /// <summary>
    /// Represents a device property for RUSTIC devices. This immutable structure encapsulates
    /// a name-value pair that describes a specific property of a RUSTIC device.
    /// </summary>
    /// <param name="name">The name of the device property.</param>
    /// <param name="value">The value associated with the device property.</param>
    public readonly struct RUSTICDeviceProperty(string name, string value)
    {
        /// <summary>
        /// Gets the name of the device property.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// Gets the value of the device property.
        /// </summary>
        public string Value { get; } = value;
        
        /// <summary>
        /// Returns a string representation of the device property in the format "Name: Value".
        /// </summary>
        /// <returns>A string containing the property name and value.</returns>
        public override string ToString() => $"{Name}: {Value}";
    }
}
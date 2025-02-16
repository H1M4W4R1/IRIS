namespace IRIS.Protocols.IRIS.Data
{
    /// <summary>
    /// Represents a device property for RUSTIC devices
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public readonly struct RUSTICDeviceProperty(string name, string value)
    {
        public string Name { get; } = name;
        public string Value { get; } = value;
        
        public override string ToString() => $"{Name}: {Value}";
    }
}
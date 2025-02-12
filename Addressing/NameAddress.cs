namespace IRIS.Addressing
{
    /// <summary>
    /// Used to access device using name
    /// </summary>
    public readonly struct NameAddress(string name) : IDeviceAddress<string>
    {
        public string Address { get; } = name;
    }
}
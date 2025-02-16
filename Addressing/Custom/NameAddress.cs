using IRIS.Addressing.Abstract;

namespace IRIS.Addressing.Custom
{
    /// <summary>
    /// Used to access device using name
    /// </summary>
    public readonly struct NameAddress(string name) : IDeviceAddress<string>
    {
        public string Address { get; } = name;
        
        public override string ToString() => Address;
    }
}
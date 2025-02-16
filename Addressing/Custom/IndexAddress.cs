using IRIS.Addressing.Abstract;

namespace IRIS.Addressing.Custom
{
    /// <summary>
    /// Used to access device using indexing (unsigned integer, 32 bit)
    /// </summary>
    public readonly struct IndexAddress(uint index) : IDeviceAddress<uint>
    {
        public uint Address { get; } = index;

        public override string ToString() => Address.ToString();
    }
}
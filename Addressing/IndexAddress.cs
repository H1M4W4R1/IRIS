namespace IRIS.Addressing
{
    /// <summary>
    /// Used to access device using indexing (unsigned integer, 32 bit)
    /// </summary>
    public readonly struct IndexAddress(uint index) : IDeviceAddress<uint>
    {
        public uint Address { get; } = index;
    }
}
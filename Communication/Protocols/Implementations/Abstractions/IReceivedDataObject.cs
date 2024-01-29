namespace IRIS.Communication.Protocols.Implementations.Abstractions
{
    public interface IReceivedDataObject<TSelf>
    {
        public static abstract TSelf Decode(byte[] data);
    }
}

using IRIS.Communication;
using IRIS.Implementations.FOCUS;
using IRIS.Implementations.RUSTIC;

namespace IRIS.Protocols
{
    /// <summary>
    /// Represents communication protocol between device and computer
    /// For a representation see <see cref="RusticProtocol"/> or <see cref="FocusProtocol"/>
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// Encode data to byte array for transmission
        /// </summary>
        public byte[] EncodeData<TData>(TData data) where TData : unmanaged;
        
        /// <summary>
        /// Try to read data from device using specified communication interface
        /// If data read fails return false, otherwise return true
        /// <see cref="data"/> should be always allocated, even if no data is read (in this case it should be empty)
        /// </summary>
        public bool TryToReadData<TData>(ICommunicationInterface communicationInterface, out TData data) where TData : unmanaged;
    }
}
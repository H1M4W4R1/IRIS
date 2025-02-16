using IRIS.Data;

namespace IRIS.Communication.Types
{
    /// <summary>
    /// Represents raw data communication interface, an interface that can
    /// send or receive raw binary data. <br/><br/>
    /// This interface overrides the default communication interface to allow
    /// easy implementation without the requirement to call any interface-based
    /// methods.
    /// </summary>
    public interface IRawDataCommunicationInterface<out TAddressType> : ICommunicationInterface<TAddressType>, IRawDataCommunicationInterface
    {
        
    }
    
    /// <summary>
    /// <see cref="IRawDataCommunicationInterface{TAddressType}"/>
    /// </summary>
    public interface IRawDataCommunicationInterface : ICommunicationInterface
    {
        /// <summary>
        /// Transmit data to device
        /// </summary>
        public DeviceResponseBase TransmitRawData(byte[] data);

        /// <summary>
        /// Read data from device
        /// </summary>
        public DeviceResponseBase ReadRawData(int length, CancellationToken cancellationToken);

        /// <summary>
        /// Read data from device until expected byte is found
        /// </summary>
        public DeviceResponseBase ReadRawDataUntil(byte expectedByte, CancellationToken cancellationToken);
    }
}
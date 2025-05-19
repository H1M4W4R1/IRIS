namespace IRIS.Communication.Types
{
    /// <summary>
    /// Represents a raw data communication interface that enables sending and receiving raw binary data.
    /// This interface extends the default communication interface to provide a simplified implementation
    /// approach that doesn't require calling interface-based methods.
    /// </summary>
    /// <typeparam name="TAddressType">The type of address used to identify the communication endpoint.</typeparam>
    public interface IRawDataCommunicationInterface<out TAddressType> : ICommunicationInterface<TAddressType>, IRawDataCommunicationInterface
    {
        
    }
    
    /// <summary>
    /// Base interface for raw data communication that defines methods for transmitting and receiving
    /// raw binary data. This interface provides the core functionality for binary data exchange
    /// between devices.
    /// </summary>
    /// <seealso cref="IRawDataCommunicationInterface{TAddressType}"/>
    public interface IRawDataCommunicationInterface : ICommunicationInterface
    {
        /// <summary>
        /// Transmits raw binary data to the connected device.
        /// </summary>
        /// <param name="data">The byte array containing the data to transmit.</param>
        /// <returns>
        /// A ValueTask containing a boolean indicating whether the transmission was successful.
        /// </returns>
        public ValueTask<bool> TransmitRawData(byte[] data);

        /// <summary>
        /// Reads a specified amount of raw binary data from the connected device.
        /// </summary>
        /// <param name="length">The number of bytes to read from the device.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the read operation.</param>
        /// <returns>
        /// A ValueTask containing a byte array with the read data.
        /// </returns>
        public ValueTask<byte[]> ReadRawData(int length, CancellationToken cancellationToken);

        /// <summary>
        /// Reads raw binary data from the connected device until a specific byte is encountered.
        /// </summary>
        /// <param name="expectedByte">The byte value that signals the end of the read operation.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the read operation.</param>
        /// <returns>
        /// A ValueTask containing a byte array with all data read up to and including the expected byte.
        /// </returns>
        public ValueTask<byte[]> ReadRawDataUntil(byte expectedByte, CancellationToken cancellationToken);
    }
}
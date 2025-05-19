using IRIS.Communication;

namespace IRIS.Protocols
{
    /// <summary>
    /// Represents a data exchange protocol that defines how data is exchanged between two systems.
    /// This interface serves as a contract for implementing specific communication protocols that handle
    /// data encoding, transmission, and decoding according to defined rules and formats.
    /// </summary>
    /// <typeparam name="TInterface">The type of communication interface used for device interaction.
    /// Must implement ICommunicationInterface.</typeparam>
    /// <typeparam name="TDataType">The type of data that will be exchanged using this protocol.</typeparam>
    public interface IProtocol<in TInterface, TDataType>
        where TInterface : ICommunicationInterface
    {
        /// <summary>
        /// Sends data to a device using the specified communication interface.
        /// </summary>
        /// <param name="communicationInterface">The communication interface to use for sending data.</param>
        /// <param name="data">The data to send to the device.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the send operation.</param>
        /// <returns>A ValueTask containing a boolean indicating whether the data was sent successfully.</returns>
        protected static abstract ValueTask<bool> SendData(
            TInterface communicationInterface,
            TDataType data,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Receives data from a device using the specified communication interface.
        /// </summary>
        /// <param name="communicationInterface">The communication interface to use for receiving data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the receive operation.</param>
        /// <returns>A ValueTask containing the received data if successful, or null if the operation failed.</returns>
        protected static abstract ValueTask<TDataType?> ReceiveData(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default);
    }
}
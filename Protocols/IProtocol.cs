using IRIS.Communication;
using IRIS.Communication.Abstract;

namespace IRIS.Protocols
{
    /// <summary>
    ///     Represents data exchange protocol - used to define how data is exchanged between two systems.
    ///     This is a representation of a data encoding and decoding format.
    /// </summary>
    public interface IProtocol<in TInterface, TDataType>
        where TInterface : ICommunicationInterface
    {
        /// <summary>
        ///       Sends data to the device using the communication interface.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="data">Data to send.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>True if the data was sent successfully, false otherwise.</returns>
        protected static abstract ValueTask<bool> SendDataAsync(TInterface communicationInterface,
            TDataType data,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///       Receives data from the device using the communication interface.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>Received data.</returns>
        protected static abstract ValueTask<TDataType> ReceiveDataAsync(TInterface communicationInterface,
            CancellationToken cancellationToken = default);
    }
}
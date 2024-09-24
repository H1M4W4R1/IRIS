using IRIS.Communication.Transactions.Abstract;
using IRIS.Protocols;

namespace IRIS.Communication
{
    /// <summary>
    /// Represents communication interface between device and computer
    /// This can be for example serial port, ethernet, etc.
    /// </summary>
    public interface ICommunicationInterface
    {
        /// <summary>
        /// Connect to physical device
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect from physical device
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send data to device
        /// </summary>
        Task SendDataAsync<TProtocol, TTransactionType, TWriteDataType>(TWriteDataType data,
            CancellationToken cancellationToken = default)
            where TProtocol : IProtocol
            where TTransactionType : ITransactionWithRequest<TTransactionType, TWriteDataType>
            where TWriteDataType : struct;
        
        Task<TResponseDataType> ReceiveDataAsync<TProtocol, TTransactionType, TResponseDataType>(
            CancellationToken cancellationToken = default)
            where TProtocol : IProtocol
            where TTransactionType : ITransactionWithResponse<TTransactionType, TResponseDataType>
            where TResponseDataType : struct;

        /// <summary>
        /// Transmit data to device
        /// </summary>
        internal void TransmitData(byte[] data);

        /// <summary>
        /// Read data from device
        /// </summary>
        /// <returns></returns>
        internal Task<byte[]> ReadData(int length, CancellationToken cancellationToken);
        
        /// <summary>
        /// Read data from device until expected byte is found
        /// </summary>
        /// <returns></returns>
        internal Task<byte[]> ReadDataUntil(byte expectedByte, CancellationToken cancellationToken);
    }
}
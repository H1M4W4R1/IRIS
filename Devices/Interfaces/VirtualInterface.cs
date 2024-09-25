using IRIS.Communication;
using IRIS.Communication.Types;
using IRIS.DataEncoders;
using IRIS.Transactions.Abstract;
using IRIS.Transactions.ReadTypes;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace IRIS.Devices.Interfaces
{
    /// <summary>
    /// Represents virtual communication interface.
    /// Can be used for testing purposes.
    /// Should have methods copied from <see cref="SerialPortInterface"/> to match its behavior.
    /// </summary>
    public abstract class VirtualInterface : ICommunicationInterface, IRawDataCommunicationInterface
    {
        /// <summary>
        /// Storage of all data received
        /// </summary>
        private readonly List<byte> _dataReceived = new List<byte>();

        /// <summary>
        /// True if device communication is available
        /// <see cref="Connect"/> and <see cref="Disconnect"/>
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Length of data received
        /// </summary>
        public int DataLength => _dataReceived.Count;

        /// <summary>
        /// Opens communication with device
        /// </summary>
        public void Connect()
        {
            IsOpen = true;
        }

        /// <summary>
        /// Closes communication with device
        /// </summary>
        public void Disconnect()
        {
            IsOpen = false;
        }

        public async Task SendDataAsync<TDataEncoder, TTransactionType, TWriteDataType>(TTransactionType transaction,
            TWriteDataType data,
            CancellationToken cancellationToken = default) where TDataEncoder : IDataEncoder
            where TTransactionType : ITransactionWithRequest<TTransactionType, TWriteDataType>
            where TWriteDataType : struct
        {
            // Get core interface
            IRawDataCommunicationInterface coreInterface = this;
            await coreInterface.DefaultSendDataAsyncImpl<TDataEncoder, TTransactionType, TWriteDataType>(
                transaction, data, cancellationToken);
        }

        public async Task<TResponseDataType> ReceiveDataAsync<TDataEncoder, TTransactionType, TResponseDataType>(
            TTransactionType transaction,
            CancellationToken cancellationToken = default) where TDataEncoder : IDataEncoder
            where TTransactionType : ITransactionWithResponse<TTransactionType, TResponseDataType>
            where TResponseDataType : struct
        {
            // Get core interface
            IRawDataCommunicationInterface coreInterface = this;
            return await coreInterface.DefaultReceiveDataAsyncImpl<TDataEncoder, TTransactionType, TResponseDataType>(
                transaction, cancellationToken);
        }

#region IRawDataCommunicationInterface

        /// <summary>
        /// Simulates transmitting data to device. See: <see cref="SimulateTransmittedData"/>.
        /// Used as proxy layer for simulating data transmission that ensures compatibility with <see cref="ICommunicationInterface"/> and
        /// <see cref="SerialPortInterface"/> which is reference for this class.
        /// </summary>
        /// <param name="data">Data to transmit</param>
        void IRawDataCommunicationInterface.TransmitRawData(byte[] data)
        {
            SimulateTransmittedData(data);
        }

        /// <summary>
        /// Reads data from device. 
        /// </summary>
        /// <param name="length">Length of data to read</param>
        /// <param name="cancellationToken">Used to cancel read operation</param>
        /// <returns>Array of data</returns>
        /// <exception cref="EndOfStreamException">If data is not long enough</exception>
        /// <exception cref="CommunicationException">If device is not open</exception>
        async Task<byte[]> IRawDataCommunicationInterface.ReadRawData(int length, CancellationToken cancellationToken)
        {
            if (_dataReceived.Count < length)
                throw new EndOfStreamException("Data is not long enough, please wait for it checking it's length");

            if (!IsOpen) throw new CommunicationException("Device is not open!");

            // Get data and remove old one
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            // Get
            return data;
        }

        /// <summary>
        /// Reads data until specified byte is found
        /// </summary>
        /// <param name="receivedByte">Byte to find</param>
        /// <param name="cancellationToken">Used to cancel read operation</param>
        /// <returns>Array of data, if byte is not found, empty array is returned</returns>
        /// <exception cref="CommunicationException">If device is not open</exception>
        async Task<byte[]> IRawDataCommunicationInterface.ReadRawDataUntil(byte receivedByte,
            CancellationToken cancellationToken)
        {
            // Check if device is open
            if (!IsOpen) throw new CommunicationException("Device is not open!");

            int dataIndex = _dataReceived.IndexOf(receivedByte);
            if (dataIndex < 0 || dataIndex > _dataReceived.Count)
                return [];

            // Get data and remove old one
            int length = dataIndex + 1;
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            return data;
        }

#endregion

        /// <summary>
        /// Simulates receiving data from device. 
        /// </summary>
        /// <param name="data">Data to receive</param>
        public void SimulateReceivedData(byte[] data)
        {
            _dataReceived.AddRange(data);
        }

        /// <summary>
        /// Simulates transmitting data to device. <br/>
        /// This is simply a mockup of device behavior, so it should be implemented in a way that simulates device behavior.
        /// </summary>
        /// <remarks>
        /// Should simulate data being processed by device and device responses - for example if device is designed to echo
        /// data back, it should be added to received data, if device is designed to multiply data by 2, then this method should
        /// take data, multiply it by 2 and add to received data.
        /// </remarks>
        public abstract void SimulateTransmittedData(byte[] data);
    }
}
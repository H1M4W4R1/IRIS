using System.IO.Ports;
using IRIS.Communication;
using IRIS.Communication.Transactions.Abstract;
using IRIS.Protocols;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace IRIS.Devices.Interfaces
{
    /// <summary>
    /// Reliable serial port, as regular one is really unreliable in data receiving.
    /// Buffers data as unbuffered event-driven solution would bend space-time continuum (quite literally).
    /// BUG: This sucks
    /// </summary>
    public sealed class SerialPortInterface : SerialPort, ICommunicationInterface
    {
        /// <summary>
        /// Used when reading data stream by single character to prevent unnecessary allocations
        /// </summary>
        private readonly byte[] _singleCharReadBuffer = new byte[1];

        public SerialPortInterface(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            PortName = portName;
            BaudRate = baudRate;
            DataBits = dataBits;
            Parity = parity;
            StopBits = stopBits;
            Handshake = Handshake.None;
            DtrEnable = false;
            RtsEnable = false;
            NewLine = Environment.NewLine;
            ReceivedBytesThreshold = 1024;
        }

        /// <summary>
        /// Connect to device - open port and start reading data
        /// </summary>
        /// <exception cref="CommunicationException">If port cannot be opened</exception>
        public void Connect()
        {
            try
            {
                // Handshake = Handshake.None;

                // Open the port
                Open();
                if (!IsOpen)
                    throw new CommunicationException("Cannot connect to device - port open failed.");
            }
            catch (UnauthorizedAccessException)
            {
                throw new CommunicationException(
                    "Cannot access device. Access has been denied. Is any software accessing this port already?");
            }
        }

        public void Disconnect() => Close();

        public async Task SendDataAsync<TProtocol, TTransactionType, TWriteDataType>(TWriteDataType data,
            CancellationToken cancellationToken = default) where TProtocol : IProtocol
            where TTransactionType : ITransactionWithRequest<TTransactionType, TWriteDataType>
            where TWriteDataType : struct
        {
            // Encode data
            byte[] encodedData = TTransactionType.Encode<TProtocol>(data);
            
            // Get core interface
            ICommunicationInterface coreInterface = this;
            
            // Transmit data
            coreInterface.TransmitData(encodedData);
        }

        public async Task<TResponseDataType> ReceiveDataAsync<TProtocol, TTransactionType, TResponseDataType>(
            CancellationToken cancellationToken = default) where TProtocol : IProtocol
            where TTransactionType : ITransactionWithResponse<TTransactionType, TResponseDataType>
            where TResponseDataType : struct
        {
            // Get core interface
            ICommunicationInterface coreInterface = this;
            
            // If transaction is based on response length, read data until it's length
            if (TTransactionType.IsByLength)
            {
                byte[] data = await coreInterface.ReadData(TTransactionType.ResponseLength, cancellationToken);

                // Decode data
                TTransactionType.Decode<TProtocol>(data, out TResponseDataType responseData);
                return responseData;
            }

            // If transaction is based on response terminator, read data until terminator is found
            if (TTransactionType.IsByEndingByte)
            {
                byte[] data = await coreInterface.ReadDataUntil(TTransactionType.ExpectedByte, cancellationToken);

                // Decode data
                TTransactionType.Decode<TProtocol>(data, out TResponseDataType responseData);
                return responseData;
            }

            // Throw exception if transaction is not supported
            throw new NotSupportedException("Transaction type is not supported");
        }

        /// <summary>
        /// Transmit data to device over serial port
        /// </summary>
        void ICommunicationInterface.TransmitData(byte[] data)
        {
            if (!IsOpen) throw new CommunicationException("Port is not open!");

            // Write data to device
            Write(data, 0, data.Length);
        }

        /// <summary>
        /// Read data from device over serial port
        /// </summary>
        /// <param name="length">Amount of data to read</param>
        /// <param name="cancellationToken">Used to cancel read operation</param>
        /// <returns></returns>
        /// <exception cref="CommunicationException">If port is not open</exception>
        async Task<byte[]> ICommunicationInterface.ReadData(int length, CancellationToken cancellationToken)
        {
            if (!IsOpen) throw new CommunicationException("Port is not open!");

            // Create buffer for data
            // TODO: Get rid of this allocation
            byte[] data = new byte[length];
            int bytesRead = 0;

            // Read data until all data is read
            while (bytesRead < length)
            {
                bytesRead += await BaseStream.ReadAsync(data, bytesRead, length - bytesRead, cancellationToken);
            }

            // Return data
            return data;
        }

        /// <summary>
        /// Reads data until specified byte is found
        /// </summary>
        /// <param name="receivedByte">Byte to find</param>
        /// <param name="cancellationToken">Used to cancel read operation</param>
        /// <returns>Array of data, if byte is not found, empty array is returned</returns>
        /// <exception cref="CommunicationException">If port is not open</exception>
        async Task<byte[]> ICommunicationInterface.ReadDataUntil(byte receivedByte, CancellationToken cancellationToken)
        {
            // Check if device is open
            if (!IsOpen) throw new CommunicationException("Port is not open!");

            // Read data until byte is found
            // TODO: Get rid of this allocation
            List<byte> data = new List<byte>();

            // Read data until byte is found
            while (true)
            {
                int bytesRead = await BaseStream.ReadAsync(_singleCharReadBuffer, 0, 1, cancellationToken);

                // Check if data is read
                if (bytesRead == 0) continue;

                // If data is read, add it to list
                data.Add(_singleCharReadBuffer[0]);

                // Check if byte is found
                if (_singleCharReadBuffer[0] == receivedByte) break;
            }

            // Return data
            return data.ToArray();
        }
    }
}
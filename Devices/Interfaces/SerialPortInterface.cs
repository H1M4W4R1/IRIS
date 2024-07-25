using System.IO.Ports;
using IRIS.Communication;

namespace IRIS.Devices.Interfaces
{
    /// <summary>
    /// Reliable serial port, as regular one is really unreliable in data receiving.
    /// Buffers data as unbuffered event-driven solution would bend space-time continuum (quite literally).
    /// </summary>
    public sealed class SerialPortInterface : SerialPort, ICommunicationInterface
    {
        /// <summary>
        /// Storage of all data received
        /// </summary>
        private readonly List<byte> _dataReceived = new List<byte>();
        
        /// <summary>
        /// Storage delegate for continuous read
        /// </summary>
        private Action? _kickoffRead = null;

        /// <summary>
        /// Amount of data currently stored
        /// </summary>
        public int DataLength => _dataReceived.Count;
        
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
                Open();
                if (!IsOpen) 
                    throw new CommunicationException("Cannot connect to device - port open failed.");
            }
            catch(UnauthorizedAccessException)
            {
                throw new CommunicationException("Cannot access device. Access has been denied. Is any software accessing this port already?");
            }
            
            ContinuousRead();
        }

        public void Disconnect() => Close();

        /// <summary>
        /// Continuous read of data from port
        /// </summary>
        private void ContinuousRead()
        {
            // 4KB buffer (static)
            byte[] buffer = new byte[4096];
            _kickoffRead = (Action)(() =>
            {
                // Only if port is open
                if (IsOpen)
                {
                    BaseStream.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar)
                    {
                        try
                        {
                            int count = BaseStream.EndRead(ar);
                            byte[] dst = new byte[count];
                            Buffer.BlockCopy(buffer, 0, dst, 0, count);
                            OnDataReceived(dst);
                        }
                        catch
                        {
                            // Do nothing
                        }

                        _kickoffRead?.Invoke();
                    }, null);
                }
            });
            _kickoffRead();
        }

        public int DataCount => _dataReceived.Count;

        /// <summary>
        /// Callback invoked when data is received
        /// </summary>
        private void OnDataReceived(IReadOnlyList<byte> data)
        {
            // Copy data to array
            for (int index = 0; index < data.Count; index++)
            {
                byte dataByte = data[index];
                _dataReceived.Add(dataByte);
            }
        }

        /// <summary>
        /// Transmit data to device over serial port
        /// </summary>
        public void TransmitData(byte[] data)
        {
            if (!IsOpen) throw new CommunicationException("Port is not open!");
            Write(data, 0, data.Length);
        }

        /// <summary>
        /// Read data from device over serial port
        /// </summary>
        /// <param name="length">Amount of data to read</param>
        /// <returns></returns>
        /// <exception cref="EndOfStreamException">If data is not long enough</exception>
        /// <exception cref="CommunicationException">If port is not open</exception>
        public byte[] ReadData(int length)
        {
            if (_dataReceived.Count < length)
                throw new EndOfStreamException("Data is not long enough, please wait for it checking it's length");

            if (!IsOpen) throw new CommunicationException("Port is not open!");

            // Get data and remove old one
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            // Get
            return data;
        }

        public byte[] PeekData(int length)
        {
            if (!IsOpen) throw new CommunicationException("Port is not open!");
            return _dataReceived.GetRange(0, length).ToArray();            
        }

        /// <summary>
        /// Reads data until specified byte is found
        /// </summary>
        /// <param name="receivedByte">Byte to find</param>
        /// <returns>Array of data, if byte is not found, empty array is returned</returns>
        public byte[] ReadDataUntil(byte receivedByte)
        {
            // Check if device is open
            if (!IsOpen) throw new CommunicationException("Port is not open!");
            
            int dataIndex = _dataReceived.IndexOf(receivedByte);
            if (dataIndex < 0 || dataIndex > _dataReceived.Count)
                return [];

            // Get data and remove old one
            int length = dataIndex + 1;
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            return data;
        }

        public bool HasByte(byte character)
        {
            return _dataReceived.Contains(character);
        }
    }
}




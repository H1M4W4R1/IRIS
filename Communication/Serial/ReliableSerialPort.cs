using IRIS.Communication.Protocols;
using IRIS.Exceptions;
using System;
using System.IO.Ports;

namespace IRIS.Communication.Serial
{
    /// <summary>
    /// Reliable serial port, as regular one is really unreliable in data receiving.
    /// Buffers data as unbuffered event-driven solution would bend space-time continuum (quite literally).
    /// </summary>
    public class ReliableSerialPort : SerialPort, IDataExchanger
    {
        private List<byte> _dataReceived = new List<byte>();
        private Action? _kickoffRead = null;

        public ReliableSerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
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

        private void ContinuousRead()
        {
            byte[]? buffer = new byte[4096];
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

                        if (_kickoffRead != null)
                            _kickoffRead();
                    }, null);
                }
            });
            _kickoffRead();
        }

        public int DataCount => _dataReceived.Count;


        public bool HasByte(byte b) => _dataReceived.Contains(b);

        public virtual void OnDataReceived(byte[] data)
        {
            // Copy data to array
            foreach (var d in data)
            {
                _dataReceived.Add(d);
            }
        }

        public void TransmitData(byte[] data)
        {
            if (!IsOpen) throw new CommunicationException("Port is not open!");

            Write(data, 0, data.Length);
        }

        public byte[] ReceiveData(int length)
        {
            if (_dataReceived.Count < length)
                throw new EndOfStreamException("Data is not long enough, please wait for it checking it's length");

            if (!IsOpen) throw new CommunicationException("Port is not open!");

            // Get data and remove old one
            var data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            // Get
            return data;
        }

        public byte[] PeekReceivedData(int length)
        {
            if (!IsOpen) throw new CommunicationException("Port is not open!");
            return _dataReceived.GetRange(0, length).ToArray();            
        }

        public byte[] ReceiveDataUntil(byte receivedByte)
        {
            var dataIndex = _dataReceived.IndexOf(receivedByte);
            if (dataIndex < 0 || dataIndex > _dataReceived.Count)
                throw new EndOfStreamException("Data is invalid. Please wait for data to be valid checking it via HasByte()");

            // Get data and remove old one
            var length = dataIndex + 1;
            var data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            return data;
        }

        public int GetLength() => DataCount;

        public void Disconnect()
        {
            Close();                
        }

        public bool IsConnected() => IsOpen;
    }
}




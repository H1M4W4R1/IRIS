using IRIS.Communication.Types;
using IRIS.Data;
using IRIS.Data.Implementations;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace IRIS.Communication
{
    /// <summary>
    /// Represents virtual communication interface.
    /// Can be used for testing purposes.
    /// </summary>
    public abstract class VirtualInterface : IRawDataCommunicationInterface
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
        public bool Connect(CancellationToken cancellationToken)
        {
            IsOpen = true;
            return true;
        }

        /// <summary>
        /// Closes communication with device
        /// </summary>
        public bool Disconnect()
        {
            IsOpen = false;
            return true;
        }

#region IRawDataCommunicationInterface

        /// <summary>
        /// Simulates transmitting data to device. See: <see cref="SimulateTransmittedData"/>.
        /// </summary>
        /// <param name="data">Data to transmit</param>
        DeviceResponseBase IRawDataCommunicationInterface.TransmitRawData(byte[] data) =>
            SimulateTransmittedData(data);

        /// <summary>
        /// Reads data from device. 
        /// </summary>
        /// <param name="length">Length of data to read</param>
        /// <param name="cancellationToken">Used to cancel read operation</param>
        DeviceResponseBase IRawDataCommunicationInterface.ReadRawData(int length, CancellationToken cancellationToken)
        {
            if (_dataReceived.Count < length) return NoResponse.Instance;

            if (!IsOpen) return NoResponse.Instance;

            // Get data and remove old one
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            // Get
            return new RawDataResponse(data);
        }

        /// <summary>
        /// Reads data until specified byte is found
        /// </summary>
        /// <param name="receivedByte">Byte to find</param>
        /// <param name="cancellationToken">Used to cancel read operation</param>
        DeviceResponseBase IRawDataCommunicationInterface.ReadRawDataUntil(byte receivedByte,
            CancellationToken cancellationToken)
        {
            // Check if device is open
            if (!IsOpen) return NoResponse.Instance;

            int dataIndex = _dataReceived.IndexOf(receivedByte);
            if (dataIndex < 0 || dataIndex > _dataReceived.Count)
                return NoResponse.Instance;

            // Get data and remove old one
            int length = dataIndex + 1;
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            return new RawDataResponse(data);
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
        public abstract DeviceResponseBase SimulateTransmittedData(byte[] data);
    }
}
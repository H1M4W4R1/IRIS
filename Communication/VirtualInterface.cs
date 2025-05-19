using IRIS.Communication.Types;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace IRIS.Communication
{
    /// <summary>
    /// Represents a virtual communication interface that simulates device communication for testing purposes.
    /// This abstract class provides a framework for implementing mock device behaviors and testing communication
    /// protocols without requiring physical hardware.
    /// </summary>
    public abstract class VirtualInterface : IRawDataCommunicationInterface
    {
        /// <summary>
        /// Internal buffer that stores all data received from the simulated device.
        /// This buffer is used to simulate the reception of data that would normally come from a physical device.
        /// </summary>
        private readonly List<byte> _dataReceived = new List<byte>();

        /// <summary>
        /// Indicates whether the virtual communication interface is currently active and ready for data exchange.
        /// This property is managed by the <see cref="Connect"/> and <see cref="Disconnect"/> methods.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Gets the total number of bytes currently stored in the receive buffer.
        /// This represents the amount of unread data available from the simulated device.
        /// </summary>
        public int DataLength => _dataReceived.Count;

        /// <summary>
        /// Simulates establishing a connection with the virtual device.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the connection attempt.</param>
        /// <returns>A ValueTask containing a boolean indicating whether the connection was successfully established.</returns>
        public ValueTask<bool> Connect(CancellationToken cancellationToken)
        {
            IsOpen = true;
            return ValueTask.FromResult(true);
        }

        /// <summary>
        /// Simulates terminating the connection with the virtual device.
        /// </summary>
        /// <returns>A ValueTask containing a boolean indicating whether the disconnection was successful.</returns>
        public ValueTask<bool> Disconnect()
        {
            IsOpen = false;
            return ValueTask.FromResult(true);
        }

#region IRawDataCommunicationInterface

        /// <summary>
        /// Simulates transmitting data to the virtual device by delegating to the <see cref="SimulateTransmittedData"/> method.
        /// This method implements the <see cref="IRawDataCommunicationInterface.TransmitRawData"/> interface method.
        /// </summary>
        /// <param name="data">The byte array containing the data to transmit to the virtual device.</param>
        /// <returns>A ValueTask containing a boolean indicating whether the transmission was successful.</returns>
        ValueTask<bool> IRawDataCommunicationInterface.TransmitRawData(byte[] data) =>
            SimulateTransmittedData(data);

        /// <summary>
        /// Simulates reading a specified amount of data from the virtual device's receive buffer.
        /// This method implements the <see cref="IRawDataCommunicationInterface.ReadRawData"/> interface method.
        /// </summary>
        /// <param name="length">The number of bytes to read from the receive buffer.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the read operation.</param>
        /// <returns>
        /// A ValueTask containing a byte array with the read data. Returns an empty array if the requested
        /// amount of data is not available or if the interface is not open.
        /// </returns>
        ValueTask<byte[]> IRawDataCommunicationInterface.ReadRawData(
            int length,
            CancellationToken cancellationToken)
        {
            if (_dataReceived.Count < length) return ValueTask.FromResult(Array.Empty<byte>());

            if (!IsOpen) return ValueTask.FromResult(Array.Empty<byte>());

            // Get data and remove old one
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            return ValueTask.FromResult(data);
        }

        /// <summary>
        /// Simulates reading data from the virtual device until a specific byte is encountered.
        /// This method implements the <see cref="IRawDataCommunicationInterface.ReadRawDataUntil"/> interface method.
        /// </summary>
        /// <param name="receivedByte">The byte value that signals the end of the read operation.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the read operation.</param>
        /// <returns>
        /// A ValueTask containing a byte array with all data read up to and including the specified byte.
        /// Returns an empty array if the specified byte is not found or if the interface is not open.
        /// </returns>
        ValueTask<byte[]> IRawDataCommunicationInterface.ReadRawDataUntil(
            byte receivedByte,
            CancellationToken cancellationToken)
        {
            // Check if device is open
            if (!IsOpen) return ValueTask.FromResult(Array.Empty<byte>());

            int dataIndex = _dataReceived.IndexOf(receivedByte);
            if (dataIndex < 0 || dataIndex > _dataReceived.Count) return ValueTask.FromResult(Array.Empty<byte>());

            // Get data and remove old one
            int length = dataIndex + 1;
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            return ValueTask.FromResult(data);
        }

#endregion

        /// <summary>
        /// Simulates receiving data from the virtual device by adding it to the internal receive buffer.
        /// This method is used to inject test data into the virtual interface's receive buffer.
        /// </summary>
        /// <param name="data">The byte array containing the data to add to the receive buffer.</param>
        public void SimulateReceivedData(byte[] data)
        {
            _dataReceived.AddRange(data);
        }

        /// <summary>
        /// Simulates the processing of transmitted data by the virtual device.
        /// This abstract method must be implemented by derived classes to define specific device behaviors.
        /// </summary>
        /// <remarks>
        /// Implementations should simulate the device's response to received data. For example:
        /// - For an echo device: Add the received data back to the receive buffer
        /// - For a processing device: Transform the data (e.g., multiply by 2) and add the result to the receive buffer
        /// - For a protocol device: Generate appropriate protocol responses based on the received data
        /// </remarks>
        /// <param name="data">The byte array containing the data to be processed by the virtual device.</param>
        /// <returns>A ValueTask containing a boolean indicating whether the data was successfully processed.</returns>
        public abstract ValueTask<bool> SimulateTransmittedData(byte[] data);
    }
}
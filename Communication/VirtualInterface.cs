using IRIS.Communication.Types;
using IRIS.Operations;
using IRIS.Operations.Abstract;
using IRIS.Operations.Connection;
using IRIS.Operations.Data;
using IRIS.Operations.Generic;
using IRIS.Utility.Awaitable;

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
        public ValueTask<IDeviceOperationResult> Connect(CancellationToken cancellationToken)
        {
            if (IsOpen) return DeviceOperation.VResult<DeviceConnectedSuccessfullyResult>();
            
            IsOpen = true;
            return DeviceOperation.VResult<DeviceConnectedSuccessfullyResult>();
        }

        /// <summary>
        /// Simulates terminating the connection with the virtual device.
        /// </summary>
        public ValueTask<IDeviceOperationResult> Disconnect()
        {
            if (!IsOpen) return DeviceOperation.VResult<DeviceDisconnectedSuccessfullyResult>();
            
            IsOpen = false;
            return DeviceOperation.VResult<DeviceDisconnectedSuccessfullyResult>();
        }

#region IRawDataCommunicationInterface

        /// <summary>
        /// Simulates transmitting data to the virtual device by delegating to the <see cref="SimulateTransmittedData"/> method.
        /// This method implements the <see cref="IRawDataCommunicationInterface.TransmitRawData"/> interface method.
        /// </summary>
        /// <param name="data">The byte array containing the data to transmit to the virtual device.</param>
        /// <returns>A ValueTask containing a boolean indicating whether the transmission was successful.</returns>
        ValueTask<IDeviceOperationResult> IRawDataCommunicationInterface.TransmitRawData(byte[] data) =>
            SimulateTransmittedData(data);

        /// <summary>
        /// Simulates reading a specified amount of data from the virtual device's receive buffer.
        /// This method implements the <see cref="IRawDataCommunicationInterface.ReadRawData"/> interface method.
        /// </summary>
        /// <param name="length">The number of bytes to read from the receive buffer.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the read operation.</param>
        async ValueTask<IDeviceOperationResult> IRawDataCommunicationInterface.ReadRawData(
            int length,
            CancellationToken cancellationToken)
        {
            // Check if device is open
            if (!IsOpen) return DeviceOperation.Result<DeviceNotConnectedResult>();
            
            // Wait until data was read
            int totalLength = await new WaitUntilCollectionExceeds<byte>(_dataReceived, length);
            
            // Check cancellation
            if (cancellationToken.IsCancellationRequested) return DeviceOperation.Result<DeviceTimeoutResult>();
 
            // Check total length
            if (totalLength < length) return DeviceOperation.Result<DeviceDataReadFailedResult>();
            
            // Get data and remove old one
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            return new DataReceivedSuccessfullyResult<byte[]>(data);
        }

        /// <summary>
        /// Simulates reading data from the virtual device until a specific byte is encountered.
        /// This method implements the <see cref="IRawDataCommunicationInterface.ReadRawDataUntil"/> interface method.
        /// </summary>
        /// <param name="receivedByte">The byte value that signals the end of the read operation.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the read operation.</param>
        async ValueTask<IDeviceOperationResult> IRawDataCommunicationInterface.ReadRawDataUntil(
            byte receivedByte,
            CancellationToken cancellationToken)
        {
            // Check if device is open
            if (!IsOpen) return DeviceOperation.Result<DeviceNotConnectedResult>();
            
            // Wait until data was read
            await new WaitUntilCollectionContains<byte>(_dataReceived, receivedByte);
            
            // Check cancellation
            if (cancellationToken.IsCancellationRequested) return DeviceOperation.Result<DeviceTimeoutResult>();

            int dataIndex = _dataReceived.IndexOf(receivedByte);
            if (dataIndex < 0 || dataIndex > _dataReceived.Count) 
                return DeviceOperation.Result<DeviceDataReadFailedResult>();

            // Get data and remove old one
            int length = dataIndex + 1;
            byte[] data = _dataReceived.GetRange(0, length).ToArray();
            _dataReceived.RemoveRange(0, length);

            return new DataReceivedSuccessfullyResult<byte[]>(data);
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
        public abstract ValueTask<IDeviceOperationResult> SimulateTransmittedData(byte[] data);
    }
}
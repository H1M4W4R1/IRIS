﻿using System.IO.Ports;
using IRIS.Communication.Types;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace IRIS.Communication.Serial
{
    /// <summary>
    /// Serial port interface for communication with devices.
    /// </summary>
    public sealed class SerialPortInterface : SerialPort, IRawDataCommunicationInterface
    {
        /// <summary>
        /// Used when reading data stream by single character to prevent unnecessary allocations
        /// </summary>
        private readonly byte[] _singleCharReadBuffer = new byte[1];

        public event Delegates.DeviceConnectionLost? OnDeviceConnectionLost;
        
        public SerialPortInterface(string portName,
            int baudRate,
            Parity parity,
            int dataBits,
            StopBits stopBits,
            bool dtrEnable,
            bool rtsEnable)
        {
            PortName = portName;
            BaudRate = baudRate;
            DataBits = dataBits;
            Parity = parity;
            StopBits = stopBits;
            Handshake = Handshake.None;
            DtrEnable = dtrEnable;
            RtsEnable = rtsEnable;
            NewLine = Environment.NewLine;
            ReceivedBytesThreshold = 1024;
        }

        /// <summary>
        /// Connect to device - open port and start reading data
        /// </summary>
        /// <exception cref="CommunicationException">If port cannot be opened</exception>
        public void Connect()
        {
            // Open the port
            Open();
            if (!IsOpen)
                throw new CommunicationException("Cannot connect to device - port open failed.");
        }

        public void Disconnect() => Close();

#region IRawDataCommunicationInterface

        /// <summary>
        /// Transmit data to device over serial port
        /// </summary>
        Task IRawDataCommunicationInterface.TransmitRawData(byte[] data)
        {
            if (!IsOpen)
            {
                OnDeviceConnectionLost?.Invoke();
                return Task.CompletedTask;
            }

            // Write data to device
            Write(data, 0, data.Length);
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// Read data from device over serial port
        /// </summary>
        /// <param name="length">Amount of data to read</param>
        /// <param name="cancellationToken">Used to cancel read operation</param>
        /// <returns></returns>
        /// <exception cref="CommunicationException">If port is not open</exception>
        async Task<byte[]> IRawDataCommunicationInterface.ReadRawData(int length, CancellationToken cancellationToken)
        {
            if (!IsOpen)
            {
                OnDeviceConnectionLost?.Invoke();
                return [];
            }

            // Create buffer for data
            // TODO: Get rid of this allocation
            byte[] data = new byte[length];
            int bytesRead = 0;

            // Read data until all data is read
            while (bytesRead < length)
            {
                bytesRead += await BaseStream.ReadAsync(data, bytesRead, length - bytesRead, cancellationToken);
                if (cancellationToken.IsCancellationRequested) break;
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
        async Task<byte[]> IRawDataCommunicationInterface.ReadRawDataUntil(byte receivedByte,
            CancellationToken cancellationToken)
        {
            // Check if device is open
            if (!IsOpen)
            {
                OnDeviceConnectionLost?.Invoke();
                return [];
            }

            // Read data until byte is found
            // TODO: Get rid of this allocation
            List<byte> data = new List<byte>();

            // Read data until byte is found
            while (true)
            {
                int bytesRead = await BaseStream.ReadAsync(_singleCharReadBuffer, 0, 1, cancellationToken);
                if (cancellationToken.IsCancellationRequested) break;
                
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

#endregion
    }
}
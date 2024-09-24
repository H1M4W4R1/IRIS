using IRIS.Addressing;
using IRIS.Communication.Transactions;
using IRIS.Devices.Interfaces;
using IRIS.Devices.Interfaces.Settings;
using IRIS.Protocols.LINE.Data;
using IRIS.Protocols.LINE.Examples;
using IRIS.Protocols.LINE.Transactions;

namespace IRIS.Examples.ArduinoEcho
{
    /// <summary>
    /// This class is an example of simple serial communication device
    /// that echoes messages back to the sender. Most commonly this example
    /// will be used with Arduino-based devices.
    /// </summary>
    public sealed class ExampleArduinoEchoDevice(
        SerialPortDeviceAddress deviceAddress,
        SerialInterfaceSettings settings) : SimpleSerialCommunicationDevice(deviceAddress, settings)
    {
        /// <summary>
        /// Exchange message with device
        /// </summary>
        public async Task<string> ExchangeMessages(string message) =>
            await ExchangeMessages<LineExchangeTransaction>(message);
        
        private async Task<string> ExchangeMessages<TTransaction>(string message)
            where TTransaction : unmanaged, IDataExchangeTransaction<TTransaction, LineTransactionData, LineTransactionData>
        {
            // Create data and transaction
            LineTransactionData data = new(message);
            
            // Exchange data
            LineTransactionData response =
                await TTransaction.ExchangeAsync<ExampleArduinoEchoDevice, SerialPortInterface>(this, data);

            // Return response
            return response.ToString();
        }
        
        /// <summary>
        /// Send message to device
        /// </summary>
        public async Task SendMessage(string message) =>
            await SendMessage<LineWriteTransaction>(message);

        private async Task SendMessage<TTransaction>(string message)
            where TTransaction : unmanaged, IWriteTransaction<TTransaction, LineTransactionData>
        {
            // Create message
            LineTransactionData data = new(message);
            
            // Send message
            await TTransaction.WriteAsync<ExampleArduinoEchoDevice, SerialPortInterface>(this, data);
        }
        
        /// <summary>
        /// Read message from device
        /// </summary>
        public async Task<string> ReadMessage() =>
            await ReadMessage<LineReadTransaction>();

        private async Task<string> ReadMessage<TTransaction>()
            where TTransaction : unmanaged, IReadTransaction<TTransaction, LineTransactionData>
        {
            // Read message
            LineTransactionData response =
                await TTransaction.ReadAsync<ExampleArduinoEchoDevice, SerialPortInterface>(this);

            // Return response
            return response.ToString();
        }
    }
}
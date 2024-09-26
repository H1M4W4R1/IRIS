using IRIS.Addressing;
using IRIS.Communication.Serial;
using IRIS.Communication.Serial.Settings;
using IRIS.DataEncoders.LINE.Data;
using IRIS.DataEncoders.LINE.Examples;
using IRIS.DataEncoders.LINE.Transactions;
using IRIS.Transactions;

namespace IRIS.Examples.Arduino.LINE_EchoApplication
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
            where TTransaction : IDataExchangeTransaction<TTransaction, LineTransactionData, LineTransactionData>, new()
        {
            // Create data and transaction
            LineTransactionData data = new(message);
            
            // Exchange data
            LineTransactionData response =
                await TTransaction.ExchangeAsync<ExampleArduinoEchoDevice, CachedSerialPortInterface>(this, data);

            // Return response
            return response.ToString();
        }
        
        /// <summary>
        /// Send message to device
        /// </summary>
        public async Task SendMessage(string message) =>
            await SendMessage<LineWriteTransaction>(message);

        private async Task SendMessage<TTransaction>(string message)
            where TTransaction : IWriteTransaction<TTransaction, LineTransactionData>, new()
        {
            // Create message
            LineTransactionData data = new(message);
            
            // Send message
            await TTransaction.WriteAsync<ExampleArduinoEchoDevice, CachedSerialPortInterface>(this, data);
        }
        
        /// <summary>
        /// Read message from device
        /// </summary>
        public async Task<string> ReadMessage() =>
            await ReadMessage<LineReadTransaction>();

        private async Task<string> ReadMessage<TTransaction>()
            where TTransaction : IReadTransaction<TTransaction, LineTransactionData>, new()
        {
            // Read message
            LineTransactionData response =
                await TTransaction.ReadAsync<ExampleArduinoEchoDevice, CachedSerialPortInterface>(this);

            // Return response
            return response.ToString();
        }
    }
}
using IRIS.Addressing;
using IRIS.Devices;
using IRIS.Devices.Interfaces;
using IRIS.Devices.Interfaces.Settings;
using IRIS.Devices.Transactions;
using IRIS.Protocols.RUSTIC.Data;
using IRIS.Protocols.RUSTIC.Transactions;
using IRIS.Transactions;

namespace IRIS.Examples.Arduino.RUSTIC_LEDApplication
{
    /// <summary>
    /// Represents an example of a device that changes or reads the value of an LED
    /// using RUSTIC protocol messages.
    /// </summary>
    public sealed class ExampleArduinoLEDChangingDevice(
        SerialPortDeviceAddress deviceAddress,
        SerialInterfaceSettings settings) : SerialDeviceBase(deviceAddress, settings),
        ISupportExchangeTransaction<ExampleArduinoLEDChangingDevice, SerialPortInterface,
            GetValueTransaction, GetValueRequestData, GetValueResponseData>,
        ISupportExchangeTransaction<ExampleArduinoLEDChangingDevice, SerialPortInterface,
            SetValueTransaction, SetValueRequestData, SetValueResponseData>
    {
        /// <summary>
        /// Get LED value
        /// </summary>
        public async Task<bool> GetLEDValue(CancellationToken cancellationToken = default) =>
            await GetLEDValue<GetValueTransaction>(cancellationToken);

        private async Task<bool> GetLEDValue<TTransactionType>(CancellationToken cancellationToken = default)
            where TTransactionType :
            IDataExchangeTransaction<GetValueTransaction, GetValueRequestData, GetValueResponseData>
        {
            // Create request data
            GetValueRequestData requestData = new("LED");

            // Exchange data
            GetValueResponseData result = await TTransactionType
                .ExchangeAsync<ExampleArduinoLEDChangingDevice, SerialPortInterface>(this, requestData,
                    cancellationToken);
            
            // Return result of the operation
            return result.value.ToString() == "1";
        }

        /// <summary>
        /// Set LED value
        /// </summary>
        public async Task<bool> SetLEDValue(bool value, CancellationToken cancellationToken = default) =>
            await SetLEDValue<SetValueTransaction>(value, cancellationToken);

        private async Task<bool> SetLEDValue<TTransactionType>(bool value, CancellationToken cancellationToken = default)
            where TTransactionType :
            IDataExchangeTransaction<SetValueTransaction, SetValueRequestData, SetValueResponseData>
        {
            // Create request data
            SetValueRequestData requestData = new("LED", value ? "1" : "0");

            // Exchange data
            SetValueResponseData result = await TTransactionType
                .ExchangeAsync<ExampleArduinoLEDChangingDevice, SerialPortInterface>(this, requestData,
                    cancellationToken);

            // Return result of the operation
            return !result.IsError;
        }
    }
}
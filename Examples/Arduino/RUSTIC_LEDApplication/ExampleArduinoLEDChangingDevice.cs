using IRIS.Addressing;
using IRIS.Addressing.Ports;
using IRIS.Communication.Serial.Settings;
using IRIS.Examples.Devices;

namespace IRIS.Examples.Arduino.RUSTIC_LEDApplication
{
    /// <summary>
    /// Represents an example of a device that changes or reads the value of an LED
    /// using RUSTIC protocol messages.
    /// </summary>
    public sealed class ExampleArduinoLEDChangingDevice(
        SerialPortDeviceAddress deviceAddress,
        SerialInterfaceSettings settings) : RUSTICDeviceBase(deviceAddress, settings)
    {
        private const string LED_PROPERTY = "LED";
        
        /// <summary>
        /// Get LED value
        /// </summary>
        public async Task<bool> GetLEDValue(CancellationToken cancellationToken = default)
        {
            return await GetProperty(LED_PROPERTY) == "1";
        }

        /// <summary>
        /// Set LED value
        /// </summary>
        public async Task<bool> SetLEDValue(bool value, CancellationToken cancellationToken = default)
        {
            // Set property to desired value
            await SetProperty(LED_PROPERTY, value ? "1" : "0");
            
            // Return new value to ensure it was set correctly
            return await GetLEDValue(cancellationToken);
        }

    }
}
using IRIS.Addressing;
using IRIS.Communication.Serial;
using IRIS.Communication.Serial.Settings;
using IRIS.Devices;
using IRIS.Protocols.IRIS;

namespace IRIS.Examples.Devices
{
    /// <summary>
    /// Base class for LINE devices
    /// Uses simple UART communication, mostly logging purposes.
    /// </summary>
    public abstract class LINEDeviceBase(
        SerialPortDeviceAddress deviceAddress,
        SerialInterfaceSettings settings) : SerialDeviceBase(deviceAddress, settings)
    {
        /// <summary>
        /// Exchange message with device
        /// </summary>
        public async Task<string> ExchangeMessages(string message) =>
            await LINE<CachedSerialPortInterface>.ExchangeMessages(HardwareAccess, message);

        /// <summary>
        /// Send message to device
        /// </summary>
        public async Task SendMessage(string message) =>
            await LINE<CachedSerialPortInterface>.SendMessage(HardwareAccess, message);

        /// <summary>
        /// Read message from device
        /// </summary>
        public async Task<string> ReadMessage() => await LINE<CachedSerialPortInterface>.ReadMessage(HardwareAccess);
    }
}
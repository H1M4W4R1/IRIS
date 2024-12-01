using System.Text;
using IRIS.Addressing;
using IRIS.Communication.Serial.Settings;
using IRIS.Devices;

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
        public async Task<string> ExchangeMessages(string message)
        {
            // Send message
            await SendMessage(message);

            // Read response
            return await ReadMessage();
        }

        /// <summary>
        /// Send message to device
        /// </summary>
        public async Task SendMessage(string message)
        {
            // Send message with embedded value
            await RawHardwareAccess.TransmitRawData(Encoding.ASCII.GetBytes(message));
            
            // Send new line
            await RawHardwareAccess.TransmitRawData(Encoding.ASCII.GetBytes("\r\n"));
        }

        /// <summary>
        /// Read message from device
        /// </summary>
        public async Task<string> ReadMessage()
        {
            // Read response
            byte[] response = await RawHardwareAccess.ReadRawDataUntil(0x0A, CancellationToken.None);
            return Encoding.ASCII.GetString(response);
        }
    }
}
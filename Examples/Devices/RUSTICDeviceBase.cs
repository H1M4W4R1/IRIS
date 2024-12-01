using System.Text;
using IRIS.Addressing;
using IRIS.Communication.Serial.Settings;
using IRIS.Devices;

namespace IRIS.Examples.Devices
{
   /// <summary>
   /// Base class for RUSTIC devices
   /// </summary>
    public abstract class RUSTICDeviceBase(
        SerialPortDeviceAddress deviceAddress,
        SerialInterfaceSettings settings) : SerialDeviceBase(deviceAddress, settings)
    {
        /// <summary>
        /// Sends SET message to device and returns the response <br/>
        /// E.g. PROPERTY to desired value
        /// </summary>
        /// <remarks>
        /// Uses ToString() method to convert <see cref="value"/> to string
        /// </remarks>
        protected async Task SetProperty<TValueType>(string message, TValueType value)
        {
            // Check if value is null, if so throw exception
            if (value == null) throw new ArgumentNullException(nameof(value));
            
            // Send message with embedded value
            await RawHardwareAccess.TransmitRawData(Encoding.ASCII.GetBytes(message));
            
            // Send request information
            await RawHardwareAccess.TransmitRawData(Encoding.ASCII.GetBytes($"={value.ToString()}"));
            
            // Send new line
            await RawHardwareAccess.TransmitRawData(Encoding.ASCII.GetBytes("\r\n"));
        }
        
        /// <summary>
        /// Sends GET message to device and returns the response <br/>
        /// </summary>
        protected async Task<string> GetProperty(string propertyName)
        {
            // Send message with embedded value
            await RawHardwareAccess.TransmitRawData(Encoding.ASCII.GetBytes(propertyName));
            
            // Send request information
            await RawHardwareAccess.TransmitRawData(Encoding.ASCII.GetBytes("=?"));
            
            // Send new line
            await RawHardwareAccess.TransmitRawData(Encoding.ASCII.GetBytes("\r\n"));
            
            // Wait for response
            byte[] response = await RawHardwareAccess.ReadRawDataUntil(0x0A, CancellationToken.None);
            
            // Return decoded response
            return Encoding.ASCII.GetString(response);
        }
    }
}
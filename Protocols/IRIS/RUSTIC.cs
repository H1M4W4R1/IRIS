using System.Text;
using IRIS.Communication.Types;
using IRIS.Protocols.IRIS.Data;
using RequestTimeout = IRIS.Utility.RequestTimeout;

namespace IRIS.Protocols.IRIS
{
    /// <summary>
    /// The RUSTIC protocol is used to exchange data between two systems in simple command-response format.
    /// </summary>
    /// <remarks>
    /// Example get command: "PROPERTY=?" <br/>
    /// Example response: "PROPERTY=VALUE" <br/>
    /// Example set command: "PROPERTY=VALUE" <br/>
    /// </remarks>
    public abstract class RUSTIC<TInterface> : IProtocol<TInterface, byte[]>
        where TInterface : IRawDataCommunicationInterface
    {
        private const string RESPONSE_GOOD = "OK";
        private const string RESPONSE_ERROR = "ERROR";
        private const string RESPONSE_TIMEOUT = "TIMEOUT";
        private const byte COMMAND_END_BYTE = 0xA;

        /// <summary>
        /// Set a property using the communication interface.
        /// </summary>
        /// <param name="propertyName">Name of the property to set.</param>
        /// <param name="propertyValue">Value of the property to set.</param>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="responseTimeout">Timeout for receiving a response (ms). If set to -1, no response is expected.</param>
        /// <typeparam name="TPropertyValue">Type of the property value.</typeparam>
        /// <returns>True if the property was set successfully, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the property name or value is null.</exception>
        /// <exception cref="TimeoutException">Thrown when a response timeout occurs.</exception>
        public static async ValueTask<RUSTICDeviceProperty?> SetProperty<TPropertyValue>(string propertyName,
            TPropertyValue propertyValue,
            TInterface communicationInterface,
            int responseTimeout = -1)
            where TPropertyValue : notnull
        {
            // Encode the property name and value into a byte array
            byte[] data = Encoding.ASCII.GetBytes($"{propertyName}={propertyValue}\r\n");

            // Send the data to the communication interface
            if(!await SendData(communicationInterface, data)) return null;

            // If no response is expected, return true
            // this is compliant with no-return option of the RUSTIC protocol
            if (responseTimeout <= 0)
                return new RUSTICDeviceProperty(propertyName, string.Empty);

            // Create a timeout for receiving a response
            RequestTimeout timeout = new(responseTimeout);

            // Receive the data from the communication interface (optional)
            // if no data is received within a certain time, the method will return
            // as if the property was not set. This does not guarantee that the property was not set as
            // the device may have set the property but the response was not received because device is
            // not sending a response which is compliant with the RUSTIC protocol no-response option.
            byte[]? receivedData = await ReceiveData(communicationInterface, timeout);
            if (timeout.IsTimedOut) return new RUSTICDeviceProperty(propertyName, RESPONSE_TIMEOUT);
            
            // Check if the received data is null
            if (receivedData == null) return null;

            // Decode the received data into a string
            string receivedString = Encoding.ASCII.GetString(receivedData);

            // Split the received string into property name and value
            string[] propertyParts = receivedString.Trim('\r', '\n').Split('=');

            string receivedPropertyName = propertyParts[0];
            string receivedPropertyValue = propertyParts[1];

            // Check if the received property name match the property name that was sent
            // also check if the received property value is OK or the same as the property value that was sent
            // if not, return false.
            // This is compliant with the RUSTIC protocol
            if (receivedPropertyName != propertyName) 
                return null;
            
            if (receivedPropertyValue == propertyValue.ToString() ||
                receivedPropertyValue == RESPONSE_GOOD) 
                return new RUSTICDeviceProperty(receivedPropertyName, receivedPropertyValue);

            // Return false if the property was not set successfully
            return null;
        }

        /// <summary>
        /// Get a property using the communication interface.
        /// </summary>
        /// <param name="propertyName">Property name to get.</param>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="responseTimeout">Timeout for receiving a response (ms). If set to -1, will wait indefinitely.</param>
        public static async ValueTask<RUSTICDeviceProperty?> GetProperty(string propertyName,
            TInterface communicationInterface,
            int responseTimeout = -1)
        {
            // Encode the property name into a byte array
            byte[] data = Encoding.ASCII.GetBytes($"{propertyName}=?\r\n");

            // Send the data to the communication interface
            if(!await SendData(communicationInterface, data)) 
                return null;

            // Receive the data from the communication interface
            RequestTimeout timeout = new(responseTimeout);
            byte[]? receivedData = await ReceiveData(communicationInterface, timeout);
            
            // Check if the response is a timeout
            if (timeout.IsTimedOut) return null;
            
            // Check if the response has data
            if (receivedData == null) return null;
            
            // Decode the received data into a string
            string receivedString = Encoding.ASCII.GetString(receivedData);

            // Split the received string into property name and value
            string[] propertyParts = receivedString.Trim('\r', '\n').Split('=');

            string receivedPropertyName = propertyParts[0];
            string propertyValue = propertyParts[1];

            // Return the received property name and value as a tuple
            return new RUSTICDeviceProperty(receivedPropertyName, propertyValue);
        }

        public static ValueTask<bool> SendData(
            TInterface communicationInterface,
            byte[] data,
            CancellationToken cancellationToken = default)
        {
            // Send the data to the communication interface
            return communicationInterface.TransmitRawData(data);
        }

        public static async ValueTask<byte[]?> ReceiveData(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            // Receive the data from the communication interface until the command end byte
            // is received
            byte[] data = await communicationInterface.ReadRawDataUntil(COMMAND_END_BYTE, cancellationToken);
            
            // Check if the response has data
            if (data.Length == 0)
                return null;
            
            // Get the received data
            return data;
        }
    }
}
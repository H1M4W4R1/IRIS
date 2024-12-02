using System.Text;
using IRIS.Communication.Types;
using IRIS.Utility;

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
        public static async Task<bool> SetProperty<TPropertyValue>(string propertyName,
            TPropertyValue propertyValue,
            TInterface communicationInterface,
            int responseTimeout = -1)
        {
            if (communicationInterface == null) throw new ArgumentNullException(nameof(communicationInterface));
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));
            if (propertyValue == null) throw new ArgumentNullException(nameof(propertyValue));

            // Encode the property name and value into a byte array
            byte[] data = Encoding.ASCII.GetBytes($"{propertyName}={propertyValue}\r\n");

            // Send the data to the communication interface
            await SendData(communicationInterface, data);

            // If no response is expected, return true
            // this is compliant with no-return option of the RUSTIC protocol
            if (responseTimeout <= 0) return true;

            // Create a timeout for receiving a response
            RequestTimeout timeout = new(responseTimeout);

            // Receive the data from the communication interface (optional)
            // if no data is received within a certain time, the method will return false
            // as if the property was not set. This does not guarantee that the property was not set as
            // the device may have set the property but the response was not received because device is
            // not sending a response which is compliant with the RUSTIC protocol no-response option.
            byte[] receivedData = await ReceiveData(communicationInterface, timeout);
            if (timeout.IsTimedOut) throw new TimeoutException("Response timeout");

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
            if (receivedPropertyName != propertyName) return false; // Handles error responses
            
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (receivedPropertyValue == propertyValue.ToString() ||
                receivedPropertyValue == RESPONSE_GOOD) return true;

            // Return false if the property was not set successfully
            return false;
        }

        /// <summary>
        /// Get a property using the communication interface.
        /// </summary>
        /// <param name="propertyName">Property name to get.</param>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="responseTimeout">Timeout for receiving a response (ms). If set to -1, will wait indefinitely.</param>
        /// <returns>Property name and value as a tuple.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the property name is null.</exception>
        /// <exception cref="TimeoutException">Thrown when a response timeout occurs.</exception>
        public static async Task<(string, string)> GetProperty(string propertyName,
            TInterface communicationInterface,
            int responseTimeout = -1)
        {
            if (communicationInterface == null) throw new ArgumentNullException(nameof(communicationInterface));
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));
            
            // Encode the property name into a byte array
            byte[] data = Encoding.ASCII.GetBytes($"{propertyName}=?\r\n");

            // Send the data to the communication interface
            await SendData(communicationInterface, data);

            // Receive the data from the communication interface
            RequestTimeout timeout = new(responseTimeout);
            byte[] receivedData = await ReceiveData(communicationInterface, timeout);
            if (timeout.IsTimedOut) throw new TimeoutException("Response timeout");

            // Decode the received data into a string
            string receivedString = Encoding.ASCII.GetString(receivedData);

            // Split the received string into property name and value
            string[] propertyParts = receivedString.Trim('\r', '\n').Split('=');

            string receivedPropertyName = propertyParts[0];
            string propertyValue = propertyParts[1];

            // Return the received property name and value as a tuple
            return (receivedPropertyName, propertyValue);
        }

        public static async Task SendData(TInterface communicationInterface,
            byte[] data,
            CancellationToken cancellationToken = default)
        {
            // Send the data to the communication interface
            await communicationInterface.TransmitRawData(data);
        }

        public static Task<byte[]> ReceiveData(TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            // Receive the data from the communication interface until the command end byte
            // is received
            return communicationInterface.ReadRawDataUntil(COMMAND_END_BYTE, cancellationToken);
        }
    }
}
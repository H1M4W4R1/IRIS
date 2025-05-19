using System.Text;
using IRIS.Communication.Types;
using IRIS.Protocols.IRIS.Data;
using RequestTimeout = IRIS.Utility.RequestTimeout;

namespace IRIS.Protocols.IRIS
{
    /// <summary>
    /// Implements the RUSTIC (Remote Universal System for Transmitting Information and Commands) protocol.
    /// This protocol provides a simple command-response format for exchanging data between systems.
    /// </summary>
    /// <remarks>
    /// The protocol follows these conventions:
    /// <list type="bullet">
    /// <item><description>Get command format: "PROPERTY=?"</description></item>
    /// <item><description>Response format: "PROPERTY=VALUE"</description></item>
    /// <item><description>Set command format: "PROPERTY=VALUE"</description></item>
    /// </list>
    /// All messages are terminated with a carriage return and line feed sequence.
    /// </remarks>
    /// <typeparam name="TInterface">The type of communication interface used for device interaction.</typeparam>
    public abstract class RUSTIC<TInterface> : IProtocol<TInterface, byte[]>
        where TInterface : IRawDataCommunicationInterface
    {
        /// <summary>
        /// Standard response indicating successful operation.
        /// </summary>
        private const string RESPONSE_GOOD = "OK";
        
        /// <summary>
        /// Standard response indicating operation failure.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private const string RESPONSE_ERROR = "ERROR";
        
        /// <summary>
        /// Standard response indicating operation timeout.
        /// </summary>
        private const string RESPONSE_TIMEOUT = "TIMEOUT";
        
        /// <summary>
        /// Byte value that marks the end of a command in the protocol.
        /// </summary>
        private const byte COMMAND_END_BYTE = 0xA;

        /// <summary>
        /// Sets a property value on the device using the RUSTIC protocol.
        /// </summary>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="propertyValue">The value to set for the property.</param>
        /// <param name="communicationInterface">The communication interface to use for device interaction.</param>
        /// <param name="responseTimeout">The maximum time to wait for a response in milliseconds. Set to -1 for no response expected.</param>
        /// <typeparam name="TPropertyValue">The type of the property value. Must be non-null.</typeparam>
        /// <returns>
        /// A ValueTask containing a RUSTICDeviceProperty if the operation was successful,
        /// or null if the operation failed. The returned property contains the confirmed property name and value.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when propertyName or propertyValue is null.</exception>
        /// <exception cref="TimeoutException">Thrown when the operation times out waiting for a response.</exception>
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
        /// Retrieves a property value from the device using the RUSTIC protocol.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        /// <param name="communicationInterface">The communication interface to use for device interaction.</param>
        /// <param name="responseTimeout">The maximum time to wait for a response in milliseconds. Set to -1 to wait indefinitely.</param>
        /// <returns>
        /// A ValueTask containing a RUSTICDeviceProperty with the property name and value if successful,
        /// or null if the operation failed or timed out.
        /// </returns>
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

        /// <summary>
        /// Sends raw data to the device using the specified communication interface.
        /// </summary>
        /// <param name="communicationInterface">The communication interface to use for sending data.</param>
        /// <param name="data">The byte array containing the data to send.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the send operation.</param>
        /// <returns>A ValueTask containing a boolean indicating whether the data was sent successfully.</returns>
        public static ValueTask<bool> SendData(
            TInterface communicationInterface,
            byte[] data,
            CancellationToken cancellationToken = default)
        {
            // Send the data to the communication interface
            return communicationInterface.TransmitRawData(data);
        }

        /// <summary>
        /// Receives raw data from the device using the specified communication interface.
        /// The method reads data until it encounters the command end byte.
        /// </summary>
        /// <param name="communicationInterface">The communication interface to use for receiving data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the receive operation.</param>
        /// <returns>
        /// A ValueTask containing the received byte array if successful,
        /// or null if no data was received or the operation failed.
        /// </returns>
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
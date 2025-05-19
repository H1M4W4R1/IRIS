using System.Text;
using IRIS.Communication.Types;

namespace IRIS.Protocols.IRIS
{
    /// <summary>
    /// Represents a LINE protocol implementation for device communication.
    /// This abstract class provides methods for sending and receiving messages using a line-based protocol
    /// where messages are terminated with a carriage return and line feed sequence.
    /// </summary>
    /// <typeparam name="TInterface">The type of communication interface used for device interaction.</typeparam>
    public abstract class LINE<TInterface> : IProtocol<TInterface, string>
        where TInterface : IRawDataCommunicationInterface
    {
        /// <summary>
        /// Sends a message to the device using the specified communication interface.
        /// </summary>
        /// <param name="communicationInterface">The communication interface to use for sending the message.</param>
        /// <param name="message">The message to send to the device.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the send operation.</param>
        /// <returns>A ValueTask containing a boolean indicating whether the message was sent successfully.</returns>
        public static ValueTask<bool> SendMessage(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
            => SendData(communicationInterface, message, cancellationToken);
        
        /// <summary>
        /// Reads a message from the device using the specified communication interface.
        /// </summary>
        /// <param name="communicationInterface">The communication interface to use for reading the message.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the read operation.</param>
        /// <returns>A ValueTask containing the received message, or null if no message was received.</returns>
        public static ValueTask<string?> ReadMessage(TInterface communicationInterface,
            CancellationToken cancellationToken = default)
            => ReceiveData(communicationInterface, cancellationToken);
        
        /// <summary>
        /// Exchanges messages with the device by sending a message and waiting for a response.
        /// </summary>
        /// <param name="communicationInterface">The communication interface to use for the exchange.</param>
        /// <param name="message">The message to send to the device.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the exchange operation.</param>
        /// <returns>A ValueTask containing the response message from the device, or null if the exchange failed.</returns>
        public static async ValueTask<string?> ExchangeMessages(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Send the message to the device
                if(!await SendMessage(communicationInterface, message, cancellationToken))
                    return null;
                
                // Return the received message
                return await ReadMessage(communicationInterface, cancellationToken);
            }
            catch(TaskCanceledException)
            {
                return null;
            }
        }

        /// <summary>
        /// Sends raw data to the device using the specified communication interface.
        /// The data is automatically terminated with a carriage return and line feed sequence.
        /// </summary>
        /// <param name="communicationInterface">The communication interface to use for sending the data.</param>
        /// <param name="data">The data to send to the device.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the send operation.</param>
        /// <returns>A ValueTask containing a boolean indicating whether the data was sent successfully.</returns>
        public static ValueTask<bool> SendData(
            TInterface communicationInterface,
            string data,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Create new string with end of line character
                string processedData = $"{data}\r\n";

                // Convert the string to a byte array
                byte[] dataBytes = Encoding.ASCII.GetBytes(processedData);

                // Send the data to the communication interface
                return communicationInterface.TransmitRawData(dataBytes);
            }
            catch(TaskCanceledException)
            {
                return ValueTask.FromResult(false);
            }
        }

        /// <summary>
        /// Receives raw data from the device using the specified communication interface.
        /// The method reads data until a line feed character (0x0A) is encountered.
        /// </summary>
        /// <param name="communicationInterface">The communication interface to use for receiving the data.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the receive operation.</param>
        /// <returns>A ValueTask containing the received data as a string, or null if no data was received.</returns>
        public static async ValueTask<string?> ReceiveData(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            // Receive the data from the communication interface until the command end byte is received
            byte[] data  = await communicationInterface.ReadRawDataUntil(0x0A, cancellationToken);
            if(data.Length == 0) return null;
            
            // Decode the received data into a string
            return Encoding.ASCII.GetString(data);
        }
    }
}
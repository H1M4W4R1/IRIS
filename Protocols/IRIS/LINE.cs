using System.Text;
using IRIS.Communication.Types;

namespace IRIS.Protocols.IRIS
{
    public abstract class LINE<TInterface> : IProtocol<TInterface, string>
        where TInterface : IRawDataCommunicationInterface
    {
        /// <summary>
        /// Send a message to the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public static async ValueTask SendMessage(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
            => await SendData(communicationInterface, message, cancellationToken);
        
        /// <summary>
        /// Read a message from the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Message from the device.</returns>
        public static async ValueTask<string> ReadMessage(TInterface communicationInterface,
            CancellationToken cancellationToken = default)
            => await ReceiveData(communicationInterface, cancellationToken);
        
        /// <summary>
        /// Exchange messages with the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response from the device.</returns>
        public static async ValueTask<string> ExchangeMessages(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
        {
            await SendMessage(communicationInterface, message, cancellationToken);
            return await ReadMessage(communicationInterface, cancellationToken);
        }

        public static async ValueTask SendData(TInterface communicationInterface,
            string data,
            CancellationToken cancellationToken = default)
        {
            // Check if the communication interface is not null
            if (communicationInterface == null) throw new ArgumentNullException(nameof(communicationInterface));
            
            // Create new string with end of line character
            string processedData = $"{data}\r\n";

            // Convert the string to a byte array
            byte[] dataBytes = Encoding.ASCII.GetBytes(processedData);

            // Send the data to the communication interface
            await communicationInterface.TransmitRawData(dataBytes);
        }

        public static async ValueTask<string> ReceiveData(TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            // Check if the communication interface is not null
            if (communicationInterface == null) throw new ArgumentNullException(nameof(communicationInterface));
            
            // Receive the data from the communication interface until the command end byte is received
            byte[] receivedData = await communicationInterface.ReadRawDataUntil(0x0A, cancellationToken);

            // Decode the received data into a string
            string receivedString = Encoding.ASCII.GetString(receivedData);

            // Return the received string
            return receivedString;
        }
    }
}
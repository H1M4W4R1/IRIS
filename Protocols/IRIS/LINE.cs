using System.Text;
using IRIS.Communication.Types;
using IRIS.Exceptions;

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
        public static ValueTask<bool> SendMessage(
            TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
            => SendData(communicationInterface, message, cancellationToken);

        /// <summary>
        /// Read a message from the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Message from the device.</returns>
        public static ValueTask<string> ReadMessage(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default)
            => ReceiveData(communicationInterface, cancellationToken);

        /// <summary>
        /// Exchange messages with the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response from the device.</returns>
        public static async ValueTask<string> ExchangeMessages(
            TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
        {
            // Send the message to the device
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!await SendMessage(communicationInterface, message, cancellationToken))
                throw new DeviceTransmissionFailed();

            // Return the received message
            return await ReadMessage(communicationInterface, cancellationToken);
        }

        public static async ValueTask<bool> SendData(
            TInterface communicationInterface,
            string data,
            CancellationToken cancellationToken = default)
        {
            // Create new string with end of line character
            string processedData = $"{data}\r\n";

            // Convert the string to a byte array
            byte[] dataBytes = Encoding.ASCII.GetBytes(processedData);

            // Send the data to the communication interface
            return await communicationInterface.TransmitRawData(dataBytes);
        }

        public static async ValueTask<string> ReceiveData(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            // Receive the data from the communication interface until the command end byte is received
            byte[] response = await communicationInterface.ReadRawDataUntil(0x0A, cancellationToken);
            if (response.Length == 0) return string.Empty;

            // Decode the received data into a string
            string receivedString = Encoding.ASCII.GetString(response);

            // Return the received string
            return receivedString;
        }
    }
}
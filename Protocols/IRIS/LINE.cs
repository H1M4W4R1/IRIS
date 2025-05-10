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
        public static ValueTask<bool> SendMessageAsync(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
            => SendDataAsync(communicationInterface, message, cancellationToken);
        
        /// <summary>
        /// Read a message from the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Message from the device.</returns>
        public static ValueTask<string?> ReadMessageAsync(TInterface communicationInterface,
            CancellationToken cancellationToken = default)
            => ReceiveDataAsync(communicationInterface, cancellationToken);
        
        /// <summary>
        /// Exchange messages with the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response from the device.</returns>
        public static async ValueTask<string?> ExchangeMessages(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Send the message to the device
                if(!await SendMessageAsync(communicationInterface, message, cancellationToken))
                    return null;
                
                // Return the received message
                return await ReadMessageAsync(communicationInterface, cancellationToken);
            }
            catch(TaskCanceledException)
            {
                return null;
            }
        }

        public static ValueTask<bool> SendDataAsync(
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

        public static async ValueTask<string?> ReceiveDataAsync(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            // Receive the data from the communication interface until the command end byte is received
            byte[] response = await communicationInterface.ReadRawDataUntil(0x0A, cancellationToken);
            if(response.Length == 0) return null;
            
            // Decode the received data into a string
            return Encoding.ASCII.GetString(response);
        }
    }
}
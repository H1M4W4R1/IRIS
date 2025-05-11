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
        public static bool SendMessage(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
            => SendData(communicationInterface, message, cancellationToken);
        
        /// <summary>
        /// Read a message from the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Message from the device.</returns>
        public static string? ReadMessage(TInterface communicationInterface,
            CancellationToken cancellationToken = default)
            => ReceiveData(communicationInterface, cancellationToken);
        
        /// <summary>
        /// Exchange messages with the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response from the device.</returns>
        public static string? ExchangeMessages(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Send the message to the device
                if(!SendMessage(communicationInterface, message, cancellationToken))
                    return null;
                
                // Return the received message
                return ReadMessage(communicationInterface, cancellationToken);
            }
            catch(TaskCanceledException)
            {
                return null;
            }
        }

        public static bool SendData(
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
                return false;
            }
        }

        public static string? ReceiveData(TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            // Receive the data from the communication interface until the command end byte is received
            byte[] data  = communicationInterface.ReadRawDataUntil(0x0A, cancellationToken);
            if(data.Length == 0) return null;
            
            // Decode the received data into a string
            return Encoding.ASCII.GetString(data);
        }
    }
}
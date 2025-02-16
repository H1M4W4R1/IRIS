using System.Text;
using IRIS.Communication.Types;
using IRIS.Data;
using IRIS.Data.Implementations;

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
        public static DeviceResponseBase SendMessage(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
            => SendData(communicationInterface, message, cancellationToken);
        
        /// <summary>
        /// Read a message from the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Message from the device.</returns>
        public static DataPromise<string> ReadMessage(TInterface communicationInterface,
            CancellationToken cancellationToken = default)
            => ReceiveData(communicationInterface, cancellationToken);
        
        /// <summary>
        /// Exchange messages with the device.
        /// </summary>
        /// <param name="communicationInterface">Communication interface to use.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response from the device.</returns>
        public static DataPromise<string> ExchangeMessages(TInterface communicationInterface,
            string message,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Send the message to the device
                // ReSharper disable once ConvertIfStatementToReturnStatement
                if(!SendMessage(communicationInterface, message, cancellationToken).IsOK)
                    return DataPromise.FromFailure(string.Empty);
                
                // Return the received message
                return ReadMessage(communicationInterface, cancellationToken);
            }
            catch(TaskCanceledException)
            {
                return DataPromise.FromFailure(string.Empty);
            }
        }

        public static DeviceResponseBase SendData(TInterface communicationInterface,
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
                return new RequestTimeout();
            }
        }

        public static DataPromise<string> ReceiveData(TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            // Receive the data from the communication interface until the command end byte is received
            DeviceResponseBase response  = communicationInterface.ReadRawDataUntil(0x0A, cancellationToken);
            if(!response.HasData<byte[]>()) return DataPromise.FromFailure(string.Empty);
            
            // Get the received data
            byte[]? receivedData = response.GetData<byte[]>();
            if(receivedData == null) return DataPromise.FromFailure(string.Empty);
            
            // Decode the received data into a string
            string receivedString = Encoding.ASCII.GetString(receivedData);

            // Return the received string
            return DataPromise.FromSuccess(receivedString);
        }
    }
}
using IRIS.Communication.Types;
using RequestTimeout = IRIS.Utility.RequestTimeout;

namespace IRIS.Protocols.IRIS
{
    /// <summary>
    /// Implements the Register Encoding Access Protocol (REAP) for device communication.
    /// This protocol provides methods for reading and writing register values on devices
    /// using a standardized byte-based communication format.
    /// </summary>
    /// <typeparam name="TInterface">The type of communication interface used for device interaction.</typeparam>
    public class REAP<TInterface> : IProtocol<TInterface, byte[]>
        where TInterface : IRawDataCommunicationInterface
    {
        /// <summary>
        /// Writes a value to a device register and optionally receives a response.
        /// </summary>
        /// <param name="communicationInterface">The communication interface used to interact with the device.</param>
        /// <param name="registerAddress">The 32-bit address of the register to write to.</param>
        /// <param name="registerValue">The 32-bit value to write to the register.</param>
        /// <param name="timeoutMs">The maximum time to wait for a response, in milliseconds.</param>
        /// <returns>
        /// A ValueTask containing the response value from the device if successful,
        /// or null if the operation failed. For devices that don't respond to write operations,
        /// returns the written value if the write was successful.
        /// </returns>
        public static async ValueTask<uint?> SetRegister(
            TInterface communicationInterface,
            uint registerAddress,
            uint registerValue,
            int timeoutMs = 100)
        {
            registerAddress |= 0x80000000; // First bit indicates write operation

            // Create address byte data
            byte[] addressByte =
            [
                (byte) ((registerAddress >> 24) & 0xFF), (byte) ((registerAddress >> 16) & 0xFF),
                (byte) ((registerAddress >> 8) & 0xFF), (byte) (registerAddress & 0xFF)
            ];

            // Create value byte data
            byte[] valueByte =
            [
                (byte) ((registerValue >> 24) & 0xFF), (byte) ((registerValue >> 16) & 0xFF),
                (byte) ((registerValue >> 8) & 0xFF), (byte) (registerValue & 0xFF)
            ];

            // Send request data
            if (!await SendData(communicationInterface, addressByte))
                return null;
            if (!await SendData(communicationInterface, valueByte)) 
                return null;

            // Receive response data
            RequestTimeout timeout = new(timeoutMs);
            byte[]? response = await ReceiveData(communicationInterface, timeout);

            // Check if timeout occurred
            // supports devices that don't respond to write operations
            if (timeout.IsTimedOut) return registerValue;

            // Check if response is valid
            if (response is not {Length: 8}) return null;

            // Parse response data
            uint responseValue =
                (uint) ((response[4] << 24) | (response[5] << 16) | (response[6] << 8) | response[7]);
            
            // Return response value
            return responseValue;
        }

        /// <summary>
        /// Reads a value from a device register.
        /// </summary>
        /// <param name="communicationInterface">The communication interface used to interact with the device.</param>
        /// <param name="registerAddress">The 32-bit address of the register to read from.</param>
        /// <param name="timeoutMs">The maximum time to wait for a response, in milliseconds.</param>
        /// <returns>
        /// A ValueTask containing the 32-bit value read from the register if successful,
        /// or null if the operation failed or timed out.
        /// </returns>
        public static async ValueTask<uint?> GetRegister(
            TInterface communicationInterface,
            uint registerAddress,
            int timeoutMs = 100)
        {
            // Create address byte data
            byte[] addressByte =
            [
                (byte) ((registerAddress >> 24) & 0xFF), (byte) ((registerAddress >> 16) & 0xFF),
                (byte) ((registerAddress >> 8) & 0xFF), (byte) (registerAddress & 0xFF)
            ];

            // Send request data
            if (!await SendData(communicationInterface, addressByte))
                return null;

            // Receive response data
            RequestTimeout timeout = new(timeoutMs);
            byte[]? response = await ReceiveData(communicationInterface, timeout);

            // Check if response is valid
            if (response == null) return null;

            // Check if timeout occurred
            if (timeout.IsTimedOut) return null;

            // Check if response is valid
            if (response is not {Length: 8}) return null;

            // Parse response data
            uint responseValue =
                (uint) ((response[4] << 24) | (response[5] << 16) | (response[6] << 8) |
                        response[7]);

            // Return response value
            return responseValue;
        }

        /// <summary>
        /// Sends raw data to the device using the specified communication interface.
        /// </summary>
        /// <param name="communicationInterface">The communication interface used to interact with the device.</param>
        /// <param name="data">The byte array containing the data to send.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the send operation.</param>
        /// <returns>A ValueTask containing a boolean indicating whether the data was sent successfully.</returns>
        public static ValueTask<bool> SendData(
            TInterface communicationInterface,
            byte[] data,
            CancellationToken cancellationToken = default)
        {
            return communicationInterface.TransmitRawData(data);
        }

        /// <summary>
        /// Receives raw data from the device using the specified communication interface.
        /// </summary>
        /// <param name="communicationInterface">The communication interface used to interact with the device.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the receive operation.</param>
        /// <returns>
        /// A ValueTask containing the received byte array if successful,
        /// or null if no data was received or the operation failed.
        /// </returns>
        public static async ValueTask<byte[]?> ReceiveData(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            byte[] data = await communicationInterface.ReadRawData(8, cancellationToken);

            // Check if response is valid
            if (data.Length == 0) return null;

            // Get the received data
            return data;
        }
    }
}
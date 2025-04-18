using IRIS.Communication.Types;
using RequestTimeout = IRIS.Utility.RequestTimeout;

namespace IRIS.Protocols.IRIS
{
    /// <summary>
    /// Register Encoding Access Protocol
    /// </summary>
    public class REAP<TInterface> : IProtocol<TInterface, byte[]>
        where TInterface : IRawDataCommunicationInterface
    {
        /// <summary>
        /// Sets device register value
        /// </summary>
        /// <param name="communicationInterface">Interface to communicate with device</param>
        /// <param name="registerAddress">Address of register to write</param>
        /// <param name="registerValue">Value to write to register</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        public static async ValueTask<uint> SetRegister(
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
                throw new CommunicationFailedException("Failed to send address data.");
            if (!await SendData(communicationInterface, valueByte))
                throw new CommunicationFailedException("Failed to send value data.");


            // Receive response data
            RequestTimeout timeout = new(timeoutMs);
            byte[] response = await ReceiveData(communicationInterface, timeout);

            // Check if timeout occurred
            // supports devices that don't respond to write operations
            if (timeout.IsTimedOut) return registerValue;

            // Parse response data
            uint responseValue =
                (uint) ((response[4] << 24) | (response[5] << 16) | (response[6] << 8) | response[7]);

            // Return response value
            return responseValue;
        }

        /// <summary>
        /// Gets device register value
        /// </summary>
        /// <param name="communicationInterface">Interface to communicate with device</param>
        /// <param name="registerAddress">Address of register to read</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>Value of register</returns>
        public static async ValueTask<uint> GetRegister(
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
                throw new CommunicationFailedException("Failed to send address data.");

            // Receive response data
            RequestTimeout timeout = new(timeoutMs);
            byte[] response = await ReceiveData(communicationInterface, timeout);

            // Check if timeout occurred
            if (timeout.IsTimedOut) throw new CommunicationFailedException("Failed to receive response data.");


            // Parse response data
            uint responseValue =
                (uint) ((response[4] << 24) | (response[5] << 16) | (response[6] << 8) |
                        response[7]);

            // Return response value
            return responseValue;
        }

        public static ValueTask<bool> SendData(
            TInterface communicationInterface,
            byte[] data,
            CancellationToken cancellationToken = default) => communicationInterface.TransmitRawData(data);

        public static async ValueTask<byte[]> ReceiveData(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default)
            => await communicationInterface.ReadRawData(8, cancellationToken);
    }
}
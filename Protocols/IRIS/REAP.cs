using IRIS.Communication.Types;
using IRIS.Utility;

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
        /// <returns>Value of register</returns>
        /// <exception cref="NotSupportedException">Response data is invalid</exception>
        public static async ValueTask<uint> SetRegister(TInterface communicationInterface, uint registerAddress, uint registerValue,
            int timeoutMs = 100)
        {
            registerAddress |= 0x80000000; // First bit indicates write operation
            
            // Create address byte data
            byte[] addressByte =
            [
                (byte)((registerAddress >> 24) & 0xFF),
                (byte)((registerAddress >> 16) & 0xFF),
                (byte)((registerAddress >> 8) & 0xFF),
                (byte)(registerAddress & 0xFF)
            ];
            
            // Create value byte data
            byte[] valueByte =
            [
                (byte)((registerValue >> 24) & 0xFF),
                (byte)((registerValue >> 16) & 0xFF),
                (byte)((registerValue >> 8) & 0xFF),
                (byte)(registerValue & 0xFF)
            ];
            
            // Send request data
            await SendData(communicationInterface, addressByte);
            await SendData(communicationInterface, valueByte);
            
            // Receive response data
            RequestTimeout timeout = new RequestTimeout(timeoutMs);
            byte[] response = await ReceiveData(communicationInterface, timeout);
            
            // Check if timeout occurred
            // supports devices that don't respond to write operations
            if (timeout.IsTimedOut)
                return registerAddress;
            
            // Check if response is valid
            if (response.Length != 8) 
                throw new NotSupportedException("Response data is invalid");
            
            // Parse response data
            uint responseValue = (uint) ((response[4] << 24) | (response[5] << 16) | (response[6] << 8) | response[7]);
            return responseValue;
        }

        /// <summary>
        /// Gets device register value
        /// </summary>
        /// <param name="communicationInterface">Interface to communicate with device</param>
        /// <param name="registerAddress">Address of register to read</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>Value of register</returns>
        /// <exception cref="TimeoutException">Timeout occurred while waiting for response</exception>
        /// <exception cref="NotSupportedException">Response data is invalid</exception>
        public static async ValueTask<uint> GetRegister(TInterface communicationInterface, uint registerAddress,
            int timeoutMs = 100)
        {
            // Create address byte data
            byte[] addressByte =
            [
                (byte)((registerAddress >> 24) & 0xFF),
                (byte)((registerAddress >> 16) & 0xFF),
                (byte)((registerAddress >> 8) & 0xFF),
                (byte)(registerAddress & 0xFF)
            ];
            
            // Send request data
            await SendData(communicationInterface, addressByte);
            
            // Receive response data
            RequestTimeout timeout = new RequestTimeout(timeoutMs);
            byte[] response = await ReceiveData(communicationInterface, timeout);
            
            // Check if timeout occurred
            if (timeout.IsTimedOut)
                throw new TimeoutException("Timeout occurred while waiting for response");
            
            // Check if response is valid
            if (response.Length != 8)
                throw new NotSupportedException("Response data is invalid");
            
            // Parse response data
            uint responseValue = (uint) ((response[4] << 24) | (response[5] << 16) | (response[6] << 8) | response[7]);
            return responseValue;
        }

        public static async ValueTask SendData(TInterface communicationInterface, byte[] data, CancellationToken cancellationToken = default)
        {
            await communicationInterface.TransmitRawData(data);
        }

        public static async ValueTask<byte[]> ReceiveData(TInterface communicationInterface, CancellationToken cancellationToken = default)
        {
            return await communicationInterface.ReadRawData(8, cancellationToken);
        }
    }
}
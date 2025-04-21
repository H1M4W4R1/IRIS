using IRIS.Communication.Abstract;
using IRIS.Exceptions;
using IRIS.Utility;
using RequestTimeout = IRIS.Utility.RequestTimeout;

namespace IRIS.Protocols.IRIS
{
    /// <summary>
    ///     Register Encoding Access Protocol
    /// </summary>
    public class REAP<TInterface> : IProtocol<TInterface, byte[]>
        where TInterface : IRawDataCommunicationInterface
    {
        /// <summary>
        ///     Sets device register value
        /// </summary>
        /// <param name="communicationInterface">Interface to communicate with device</param>
        /// <param name="registerAddress">Address of register to write</param>
        /// <param name="registerValue">Value to write to register</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        public static uint SetRegister(
            TInterface communicationInterface,
            uint registerAddress,
            uint registerValue,
            int timeoutMs = 100) =>
            SetRegisterAsync(communicationInterface, registerAddress, registerValue, timeoutMs).Wait();
        
        /// <summary>
        ///     Gets device register value
        /// </summary>
        /// <param name="communicationInterface">Interface to communicate with device</param>
        /// <param name="registerAddress">Address of register to read</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>Value of register</returns>
        public static uint GetRegister(
            TInterface communicationInterface,
            uint registerAddress,
            int timeoutMs = 100) =>
            GetRegisterAsync(communicationInterface, registerAddress, timeoutMs).Wait();
        
        /// <summary>
        ///     Sets device register value
        /// </summary>
        /// <param name="communicationInterface">Interface to communicate with device</param>
        /// <param name="registerAddress">Address of register to write</param>
        /// <param name="registerValue">Value to write to register</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        public static async ValueTask<uint> SetRegisterAsync(
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
            if (!await SendDataAsync(communicationInterface, addressByte)) throw new DeviceTransmissionFailed();
            if (!await SendDataAsync(communicationInterface, valueByte)) throw new DeviceTransmissionFailed();

            // Receive response data
            RequestTimeout timeout = new(timeoutMs);
            byte[] response = await ReceiveDataAsync(communicationInterface, timeout);

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
        ///     Gets device register value
        /// </summary>
        /// <param name="communicationInterface">Interface to communicate with device</param>
        /// <param name="registerAddress">Address of register to read</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>Value of register</returns>
        public static async ValueTask<uint> GetRegisterAsync(
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
            if (!await SendDataAsync(communicationInterface, addressByte)) throw new DeviceTransmissionFailed();

            // Receive response data
            RequestTimeout timeout = new(timeoutMs);
            byte[] response = await ReceiveDataAsync(communicationInterface, timeout);

            // Check if timeout occurred
            if (timeout.IsTimedOut) throw new ResponseTimeoutException();


            // Parse response data
            uint responseValue =
                (uint) ((response[4] << 24) | (response[5] << 16) | (response[6] << 8) |
                        response[7]);

            // Return response value
            return responseValue;
        }

        public static ValueTask<bool> SendDataAsync(
            TInterface communicationInterface,
            byte[] data,
            CancellationToken cancellationToken = default) => communicationInterface.TransmitRawDataAsync(data);

        public static async ValueTask<byte[]> ReceiveDataAsync(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default)
            => await communicationInterface.ReadRawDataAsync(8, cancellationToken);
    }
}
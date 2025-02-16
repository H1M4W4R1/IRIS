using IRIS.Communication.Types;
using IRIS.Data;
using IRIS.Data.Implementations;
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
        public static DataPromise<uint> SetRegister(
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
            if (!SendData(communicationInterface, addressByte).IsOK)
                return DataPromise<uint>.FromFailure(registerAddress);
            if (!SendData(communicationInterface, valueByte).IsOK) 
                return DataPromise<uint>.FromFailure(registerAddress);

            // Receive response data
            RequestTimeout timeout = new(timeoutMs);
            DataPromise<byte[]> response = ReceiveData(communicationInterface, timeout);

            // Check if timeout occurred
            // supports devices that don't respond to write operations
            if (timeout.IsTimedOut) return DataPromise<uint>.FromSuccess(registerValue);

            // Check if response is valid
            if (response.Data is not {Length: 8}) return DataPromise<uint>.FromFailure(registerAddress);

            // Parse response data
            uint responseValue =
                (uint) ((response.Data[4] << 24) | (response.Data[5] << 16) | (response.Data[6] << 8) | response.Data[7]);
            
            // Return response value
            return DataPromise<uint>.FromSuccess(responseValue);
        }

        /// <summary>
        /// Gets device register value
        /// </summary>
        /// <param name="communicationInterface">Interface to communicate with device</param>
        /// <param name="registerAddress">Address of register to read</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>Value of register</returns>
        public static DataPromise<uint> GetRegister(
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
            if (SendData(communicationInterface, addressByte) is not OKResponse)
                return DataPromise.FromFailure<uint>();

            // Receive response data
            RequestTimeout timeout = new(timeoutMs);
            DataPromise<byte[]> response = ReceiveData(communicationInterface, timeout);

            // Check if promise is successful
            if (!response.IsSuccess) return DataPromise.FromFailure<uint>();

            // Check if timeout occurred
            if (timeout.IsTimedOut) return DataPromise.FromFailure<uint>();

            // Check if response is valid
            if (response.Data is not {Length: 8}) return DataPromise.FromFailure<uint>();

            // Parse response data
            uint responseValue =
                (uint) ((response.Data[4] << 24) | (response.Data[5] << 16) | (response.Data[6] << 8) |
                        response.Data[7]);

            // Return response value
            return DataPromise.FromSuccess(responseValue);
        }

        public static DeviceResponseBase SendData(
            TInterface communicationInterface,
            byte[] data,
            CancellationToken cancellationToken = default)
        {
            return communicationInterface.TransmitRawData(data);
        }

        public static DataPromise<byte[]> ReceiveData(
            TInterface communicationInterface,
            CancellationToken cancellationToken = default)
        {
            DeviceResponseBase response = communicationInterface.ReadRawData(8, cancellationToken);

            // Check if response is valid
            if (!response.HasData<byte[]>()) return DataPromise<byte[]>.FromFailure();

            // Get the received data
            byte[]? receivedData = response.GetData<byte[]>();

            return receivedData == null
                ? DataPromise<byte[]>.FromFailure()
                : DataPromise.FromSuccess(receivedData);
        }
    }
}
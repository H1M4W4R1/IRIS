using System.Data;
using IRIS.Communication;
using IRIS.Implementations.FOCUS.Data;
using IRIS.Protocols;
using IRIS.Utility;

namespace IRIS.Implementations.FOCUS
{
    /// <summary>
    /// FOCUS protocol implementation <br/><br/>
    /// FOCUS protocol is a command-response protocol which is used to communicate with external devices
    /// </summary>
    /// <remarks>
    /// User sends command bytes to device and waits for response of fixed length or error response with length of 4 bytes
    /// All lengths are determined by command and include status byte and ENDL byte.
    /// If status byte is not <see cref="FocusResponseCodes.RESPONSE_OK"/>, then response is error response followed
    /// by 2 bytes of error message and ENDL byte
    /// </remarks>
    public struct FocusProtocol : IProtocol, IUnmanaged<FocusProtocol>
    {
        /// <summary>
        /// Encodes the provided data into a byte array.
        /// </summary>
        /// <typeparam name="TData">The type of data to encode. Must be unmanaged.</typeparam>
        /// <param name="data">The data to encode.</param>
        /// <returns>A byte array representing the encoded data.</returns>
        /// <remarks>
        /// This method attempts to cast the provided data to the <see cref="IFocusCommand"/> type.
        /// If the cast is successful, it retrieves the RequestBytes property from the <see cref="IFocusCommand"/> object and returns it.
        /// If the cast is not successful, a runtime exception will be thrown.
        /// <b>Developer should ensure that the provided data is of the correct type.</b>
        /// </remarks>
        public byte[] EncodeData<TData>(TData data) where TData : unmanaged
        {
            IFocusCommand focusCommand = (IFocusCommand) data;
            return focusCommand.RequestBytes;
        }

        /// <summary>
        /// Tries to read data from the communication interface.
        /// </summary>
        /// <typeparam name="TData">The type of data to read. Must be unmanaged.</typeparam>
        /// <param name="communicationInterface">The communication interface to read data from.</param>
        /// <param name="data">Output parameter where the read data will be stored if the operation is successful.</param>
        /// <returns>True if the data was successfully read and is of the correct type, false otherwise.</returns>
        /// <remarks>
        /// This method attempts to read data of type TData from the provided communication interface.
        /// If the data is not of type <see cref="IFocusCommand"/>, a <see cref="ConstraintException"/> is thrown.
        /// The method first checks if the length of the response from the communication interface is valid.
        /// If it is not, the method returns false.
        /// The method then peeks at the response code from the communication interface.
        /// If the response code is not RESPONSE_OK, the response length is set to 4.
        /// The method checks again if the response length is valid. If it is not, the method returns false.
        /// The method then reads the data from the communication interface and decodes the response.
        /// If all these operations are successful, the method returns true.
        /// It may throw exception during decoding if the data decoding fails for any reason.
        /// </remarks>
        public bool TryToReadData<TData>(ICommunicationInterface communicationInterface, out TData data)
            where TData : unmanaged
        {
            // Temporary object to acquire data, also default value for return
            data = new TData();

            if (data is not IFocusCommand focusCommand)
                throw new ConstraintException("Data is not of type IFocusCommand");

            // Acquire length of response
            int responseLength = focusCommand.ResponseLength;

            // Check if response length is valid
            if (communicationInterface.DataLength < responseLength)
                return false;
            
            // Peek response code
            byte[] response = communicationInterface.PeekData(1);
            if(response[0] != FocusResponseCodes.RESPONSE_OK)
                responseLength = 4;
            
            // Check again if response length is valid - if not, return false
            if(communicationInterface.DataLength < responseLength)
                return false;
            
            // Read data
            byte[] responseData = communicationInterface.ReadData(responseLength);

            // Decode response 
            focusCommand.Decode<TData>(responseData);
            return true;
        }
    }
}
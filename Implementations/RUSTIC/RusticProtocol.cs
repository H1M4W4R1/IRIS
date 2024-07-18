using System.Data;
using System.Text;
using IRIS.Communication;
using IRIS.Implementations.RUSTIC.Data;
using IRIS.Protocols;
using IRIS.Utility;

namespace IRIS.Implementations.RUSTIC
{
    /// <summary>
    /// RUSTIC protocol implementation <br/><br/>
    /// RUSTIC protocol is ASCII-based protocol that is used for communication with devices.
    /// </summary>
    /// <remarks>
    /// This protocol sends commands via simple assignments, if value is assigned to question mark then it means that device should return value.
    /// If value is assigned to something else, it means that device should set value.
    /// </remarks>
    public struct RusticProtocol : IProtocol, IUnmanaged<RusticProtocol>
    {
        /// <summary>
        /// Encodes the provided data into a byte array.
        /// </summary>
        /// <typeparam name="TData">The type of data to encode. Must be unmanaged.</typeparam>
        /// <param name="data">The data to encode.</param>
        /// <returns>A byte array representing the encoded data.</returns>
        /// <remarks>
        /// This method attempts to cast the provided data to the <see cref="IRusticCommand"/> type.
        /// If the cast is successful, it encodes the command into an ASCII string based on the command type and returns it as a byte array.
        /// If the command type is not supported, a <see cref="NotSupportedException"/> is thrown.
        /// If the data is not of type <see cref="IRusticCommand"/>, a <see cref="ConstraintException"/> is thrown.
        /// </remarks>
        public byte[] EncodeData<TData>(TData data) where TData : unmanaged
        {
            // Check if data is of type IRusticCommand
            if (data is IRusticCommand command)
            {
                // Based on command type encode data to ASCII string and send it
                return command.CommandType switch
                {
                    RusticCommandType.Get => Encoding.ASCII.GetBytes(command.Identifier + "=?\r\n"),
                    RusticCommandType.Set => Encoding.ASCII.GetBytes(command.Identifier + "=" + command.Encode() +
                                                                     "\r\n"),
                    _ => throw new NotSupportedException("Command type not supported")
                };
            }

            throw new ConstraintException("Data is not of type IRusticCommand");
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
        /// If the data is not of type <see cref="IRusticCommand"/>, a <see cref="ConstraintException"/> is thrown.
        /// The method first checks if the communication interface has an end of line byte (<i>0xA</i>).
        /// If it does not, the method returns false.
        /// The method then reads data until the end of line from the communication interface and decodes the response.
        /// If all these operations are successful, the method returns true.
        /// </remarks>
        public bool TryToReadData<TData>(ICommunicationInterface communicationInterface, out TData data)
            where TData : unmanaged
        {
            // Set default value for return
            data = new TData();

            // Ensure that data received has end of line (0xA aka. '\n')
            if (!communicationInterface.HasByte(0xA)) return false;

            // Ensure that data is of type IRusticCommand
            if (data is not IRusticCommand command) throw new ConstraintException("Data is not of type IRusticCommand");

            // Read data until end of line
            byte[] response = communicationInterface.ReadDataUntil(0xA);

            // Decode response
            command.Decode<TData>(response);

            return true;
        }
    }
}
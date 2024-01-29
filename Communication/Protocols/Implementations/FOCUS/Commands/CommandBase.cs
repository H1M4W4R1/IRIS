using IRIS.Communication.Protocols.Implementations.Abstractions;

namespace IRIS.Communication.Protocols.Implementations.FOCUS.Commands
{
    public abstract class CommandBase<T> : InvokableCommand<T> where T : IReceivedDataObject<T>
    {
        private const byte RESPONSE_OK = 0x1;
        private int _responseLength = 2; // [OK, 0xA]

        /// <summary>
        /// Create command byte array
        /// </summary>
        public abstract byte[] GetCommandBytes();

        protected override T Execute(IProtocol protocol, IDataExchanger exchanger)
        {
            // Send data
            exchanger.TransmitData(GetCommandBytes());

            // Wait for response result
            var targetLength = _responseLength;
            while (exchanger.GetLength() < 1) ;

            // Check if result is OK, if not then it's 4-byte error code ([ERROR, PARAM1, PARAM2, 0xA]).
            var result = exchanger.PeekReceivedData(1)[0];
            if (result != RESPONSE_OK)
                targetLength = 4;

            // Wait for command to be processed properly
            while (exchanger.GetLength() < targetLength) ;

            // Read data
            var response = exchanger.ReceiveData(targetLength);

            // Decode response
            return T.Decode(response);
        }


        public CommandBase(int responseLength)
        {
            _responseLength = responseLength;
        }
    }
}

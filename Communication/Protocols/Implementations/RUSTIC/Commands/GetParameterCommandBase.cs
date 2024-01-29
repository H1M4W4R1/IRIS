using IRIS.Communication.Protocols.Implementations.Abstractions;
using System.Text;

namespace IRIS.Communication.Protocols.Implementations.RUSTIC.Commands
{
    public class GetParameterCommandBase<T> : InvokableCommand<T> where T: IReceivedDataObject<T>
    {
        private string _id;

        protected override T Execute(IProtocol protocol, IDataExchanger exchanger)
        {
            var asciiBytes = Encoding.ASCII.GetBytes(_id + "=?\r\n");

            // Send data
            exchanger.TransmitData(asciiBytes);

            // Wait for End of Line
            while (!exchanger.HasByte(0xA));

            // Read data
            var response = exchanger.ReceiveDataUntil(0xA);

            // Decode response
            return T.Decode(response);
        }

        public GetParameterCommandBase(string id)
        {
            _id = id;
        }

        /// <summary>
        /// Disabled constructor access
        /// </summary>
        private GetParameterCommandBase()
        {
            _id = "UNKNOWN";
        }


    }
}

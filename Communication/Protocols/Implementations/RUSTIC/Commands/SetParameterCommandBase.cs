using IRIS.Communication.Protocols.Implementations.Abstractions;
using IRIS.Communication.Protocols.Implementations.RUSTIC.Status;
using System.Text;

namespace IRIS.Communication.Protocols.Implementations.RUSTIC.Commands
{
    public class SetParameterCommandBase : InvokableCommand<StatusObject>
    {
        private string _id;

        protected override StatusObject Execute(IProtocol protocol, IDataExchanger exchanger)
        {
            var asciiBytes = Encoding.ASCII.GetBytes(_id + "=?\r\n");

            // Send data
            exchanger.TransmitData(asciiBytes);

            // Wait for End of Line
            while (!exchanger.HasByte(0xA));

            // Read data
            var response = exchanger.ReceiveDataUntil(0xA);

            // Decode response
            return StatusObject.Decode(response);
        }

        public SetParameterCommandBase(string id)
        {
            _id = id;
        }

        /// <summary>
        /// Disabled constructor access
        /// </summary>
        private SetParameterCommandBase()
        {
            _id = "UNKNOWN";
        }


    }
}

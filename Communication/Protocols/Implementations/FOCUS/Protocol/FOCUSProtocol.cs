using IRIS.Communication.Protocols.Recognition;
using IRIS.Exceptions;

namespace IRIS.Communication.Protocols.Implementations.FOCUS.Protocol
{
    public class FOCUSProtocol : IProtocol<FOCUSProtocol>
    {
        private IDataExchanger? _dataExchanger;
        private IDeviceRecognizer? _deviceRecognizer;

        public FOCUSProtocol Build()
        {
            return this;
        }

        public IDataExchanger? GetDataExchanger()
        {
            return _dataExchanger;
        }

        public IDeviceRecognizer? GetDeviceRecognizer()
        {
            return _deviceRecognizer;
        }

        public FOCUSProtocol UsesDataExchanger(IDataExchanger dataExchanger)
        {
            _dataExchanger = dataExchanger;
            return this;
        }

        public FOCUSProtocol Validate()
        {
            if(_dataExchanger == null)
            {
                throw new InvalidException("Data exchanger has not been found");
            }

            return this;
        }
    }
}

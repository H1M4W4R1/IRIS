using IRIS.Communication.Protocols.Recognition;
using IRIS.Exceptions;

namespace IRIS.Communication.Protocols.Implementations.FOCUS.Protocol
{
    public class RUSTICProtocol : IProtocol<RUSTICProtocol>
    {
        private IDataExchanger? _dataExchanger;
        private IDeviceRecognizer? _deviceRecognizer;

        public RUSTICProtocol Build()
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

        public RUSTICProtocol UsesDataExchanger(IDataExchanger dataExchanger)
        {
            _dataExchanger = dataExchanger;
            return this;
        }

        public RUSTICProtocol Validate()
        {
            if(_dataExchanger == null)
            {
                throw new InvalidException("Data exchanger has not been found");
            }

            return this;
        }
    }
}

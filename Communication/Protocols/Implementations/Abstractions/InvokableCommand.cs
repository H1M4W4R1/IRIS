using IRIS.Devices;
using IRIS.Exceptions;

namespace IRIS.Communication.Protocols.Implementations.Abstractions
{
    public abstract class InvokableCommand<T> where T : IReceivedDataObject<T>
    {
        public T ExecuteOn(IDevice device)
        {
            // Get protocol
            var protocol = device.GetCurrentProtocol();
            if (protocol == null)
                throw new ExecutionException("Device is not connected properly.");

            var dataExchanger = protocol.GetDataExchanger();
            if (dataExchanger == null)
                throw new ExecutionException("Device communication method is not known.");

            // Send and receive data (process internal function)
            T result = Execute(protocol, dataExchanger);

            return result;
        }

        /// <summary>
        /// Command execution process
        /// </summary>
        protected abstract T Execute(IProtocol protocol, IDataExchanger exchanger);
    }
}

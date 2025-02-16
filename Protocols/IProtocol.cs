using IRIS.Communication;
using IRIS.Data;

namespace IRIS.Protocols
{
    /// <summary>
    /// Represents data exchange protocol - used to define how data is exchanged between two systems.
    /// This is a representation of a data encoding and decoding format.
    /// </summary>
    public interface IProtocol<in TInterface, TDataType>
        where TInterface : ICommunicationInterface
    {
        protected static abstract DeviceResponseBase SendData(TInterface communicationInterface,
            TDataType data,
            CancellationToken cancellationToken = default);

        protected static abstract DataPromise<TDataType> ReceiveData(TInterface communicationInterface,
            CancellationToken cancellationToken = default);
    }
}
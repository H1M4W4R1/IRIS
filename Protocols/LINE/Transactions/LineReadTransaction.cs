using IRIS.Communication;
using IRIS.Devices;
using IRIS.Protocols.LINE.Abstract;
using IRIS.Protocols.LINE.Data;
using IRIS.Transactions;

namespace IRIS.Protocols.LINE.Transactions
{
    public struct LineReadTransaction : IReadTransaction<LineReadTransaction, LineTransactionData>, ILineTransaction
    {
        public async Task<LineTransactionData> _ReadAsync<TDevice, TCommunicationInterface>(
            TDevice device, CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface> where TCommunicationInterface : ICommunicationInterface
        {
            // Get the communication interface and receive data
            ICommunicationInterface communicationInterface = device.GetCommunicationInterface();
            return await communicationInterface
                .ReceiveDataAsync<LineProtocol, LineReadTransaction, LineTransactionData>(this, cancellationToken);
        }
    }
}
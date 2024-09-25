using IRIS.Communication;
using IRIS.DataEncoders.LINE.Abstract;
using IRIS.DataEncoders.LINE.Data;
using IRIS.Devices;
using IRIS.Transactions;

namespace IRIS.DataEncoders.LINE.Transactions
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
                .ReceiveDataAsync<LineDataEncoder, LineReadTransaction, LineTransactionData>(this, cancellationToken);
        }
    }
}
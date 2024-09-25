using IRIS.Communication;
using IRIS.DataEncoders.LINE.Abstract;
using IRIS.DataEncoders.LINE.Data;
using IRIS.Devices;
using IRIS.Transactions;

namespace IRIS.DataEncoders.LINE.Transactions
{
    public struct LineWriteTransaction : IWriteTransaction<LineWriteTransaction, LineTransactionData>, ILineTransaction
    {
        public async Task<bool> _WriteAsync<TDevice, TCommunicationInterface>(TDevice device,
            LineTransactionData requestData,
            CancellationToken cancellationToken = default) where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            // Get the communication interface and send data
            ICommunicationInterface communicationInterface = device.GetCommunicationInterface();
            await communicationInterface.SendDataAsync<LineDataEncoder, LineWriteTransaction, LineTransactionData>(this, requestData, cancellationToken);
            return true;
        }
    }
}
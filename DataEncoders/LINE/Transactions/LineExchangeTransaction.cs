using IRIS.Communication;
using IRIS.DataEncoders.LINE.Abstract;
using IRIS.DataEncoders.LINE.Data;
using IRIS.Devices;
using IRIS.Transactions;

namespace IRIS.DataEncoders.LINE.Transactions
{
    public struct LineExchangeTransaction :
        IDataExchangeTransaction<LineExchangeTransaction, LineTransactionData, LineTransactionData>, ILineTransaction
    {
        public async Task<LineTransactionData> _ExchangeAsync<TDevice, TCommunicationInterface>(TDevice device,
            LineTransactionData requestData,
            CancellationToken cancellationToken = default) where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            // Get communication interface
            ICommunicationInterface communicationInterface = device.GetCommunicationInterface();
            
            // Send data to device
            await communicationInterface.SendDataAsync(this, requestData, cancellationToken);
            
            // Read response from device
            return await communicationInterface
                .ReceiveDataAsync<LineExchangeTransaction, LineTransactionData>(this, cancellationToken);
        }
    }
}
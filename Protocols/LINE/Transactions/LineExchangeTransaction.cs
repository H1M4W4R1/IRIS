using IRIS.Communication;
using IRIS.Devices;
using IRIS.Protocols.LINE.Abstract;
using IRIS.Protocols.LINE.Data;
using IRIS.Transactions;

namespace IRIS.Protocols.LINE.Transactions
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
            await communicationInterface.SendDataAsync<LineProtocol, LineExchangeTransaction, LineTransactionData>(this, requestData, cancellationToken);
            
            // Read response from device
            return await communicationInterface
                .ReceiveDataAsync<LineProtocol, LineExchangeTransaction, LineTransactionData>(this, cancellationToken);
        }
    }
}
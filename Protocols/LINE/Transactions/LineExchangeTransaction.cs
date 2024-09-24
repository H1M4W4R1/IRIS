using IRIS.Communication;
using IRIS.Communication.Transactions;
using IRIS.Devices;
using IRIS.Protocols.LINE.Abstract;
using IRIS.Protocols.LINE.Data;

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
            await communicationInterface.SendDataAsync<LineProtocol, LineExchangeTransaction, LineTransactionData>(
                requestData, cancellationToken);
            
            // Read response from device
            return await communicationInterface
                .ReceiveDataAsync<LineProtocol, LineExchangeTransaction, LineTransactionData>(cancellationToken);
        }
    }
}
using IRIS.Communication;
using IRIS.Devices;
using IRIS.Protocols.RUSTIC.Data;
using IRIS.Transactions;
using IRIS.Transactions.ReadTypes;

namespace IRIS.Protocols.RUSTIC.Transactions
{
    /// <summary>
    /// Represents a transaction that sets a value in the device.
    /// </summary>
    public struct SetValueTransaction : ITransactionReadUntilByte,
        IDataExchangeTransaction<SetValueTransaction, SetValueRequestData, SetValueResponseData>
    {
        /// <summary>
        /// Byte that indicates the end of the response.
        /// </summary>
        public byte ExpectedByte => 0x0A;

        public async Task<SetValueResponseData> _ExchangeAsync<TDevice, TCommunicationInterface>(TDevice device,
            SetValueRequestData requestData,
            CancellationToken cancellationToken = default) where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            // Get communication interface
            TCommunicationInterface communicationInterface = device.GetCommunicationInterface();

            // Send request to set value
            await communicationInterface.SendDataAsync<RusticProtocol, SetValueTransaction, SetValueRequestData>(this,
                requestData, cancellationToken);

            // Read response
            return await communicationInterface
                .ReceiveDataAsync<RusticProtocol, SetValueTransaction, SetValueResponseData>(this, cancellationToken);
        }
    }
}
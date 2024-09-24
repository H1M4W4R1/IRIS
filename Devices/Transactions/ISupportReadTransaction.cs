using IRIS.Communication;
using IRIS.Transactions;
using IRIS.Transactions.Abstract;

namespace IRIS.Devices.Transactions
{
    /// <summary>
    /// Represents that device supports specific read transaction
    /// </summary>
    public interface ISupportReadTransaction<
        TDevice, TCommunicationInterface,
        in TReadTransactionType, TResponseDataType>
        where TReadTransactionType : IReadTransaction<TReadTransactionType, TResponseDataType>,
        ITransactionWithResponse<TReadTransactionType, TResponseDataType>, new()
        where TResponseDataType : struct
        where TDevice : DeviceBase<TCommunicationInterface>
        where TCommunicationInterface : ICommunicationInterface
    {
        /// <summary>
        /// Reads data from device
        /// </summary>
        public async Task<TResponseDataType> ReadAsync(CancellationToken cancellationToken = default) =>
            await TReadTransactionType.ReadAsync<TDevice, TCommunicationInterface>((TDevice) this, cancellationToken);
        
        /// <summary>
        /// Reads data from device using specific transaction
        /// </summary>
        public async Task<TResponseDataType> ReadAsync(TReadTransactionType transaction, CancellationToken cancellationToken = default) =>
            await transaction._ReadAsync<TDevice, TCommunicationInterface>((TDevice) this, cancellationToken);
    }
}
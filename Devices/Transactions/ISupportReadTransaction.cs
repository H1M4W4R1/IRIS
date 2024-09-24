using IRIS.Communication;
using IRIS.Communication.Transactions;
using IRIS.Communication.Transactions.Abstract;

namespace IRIS.Devices.Transactions
{
    /// <summary>
    /// Represents that device supports specific read transaction
    /// </summary>
    public interface ISupportReadTransaction<
        TDevice, TCommunicationInterface,
        in TReadTransactionType, TResponseDataType>
        where TReadTransactionType : unmanaged, IReadTransaction<TReadTransactionType, TResponseDataType>,
        ITransactionWithResponse<TReadTransactionType, TResponseDataType>
        where TResponseDataType : unmanaged
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
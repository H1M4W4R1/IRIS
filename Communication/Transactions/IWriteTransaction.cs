using IRIS.Communication.Transactions.Abstract;
using IRIS.Devices;

namespace IRIS.Communication.Transactions
{
    /// <summary>
    /// Represents device write request transaction.
    /// </summary>
    public interface IWriteTransaction<TSelf, in TDataType> : ITransactionWithRequest<TSelf, TDataType>
        where TSelf : unmanaged, IWriteTransaction<TSelf, TDataType>
        where TDataType : unmanaged
    {
        /// <summary>
        /// Implement device write transaction logic here.
        /// </summary>
        public Task<bool> _WriteAsync<TDevice, TCommunicationInterface>(TDevice device, TDataType requestData,
            CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface;
        
        /// <summary>
        /// This method performs write transaction between device and computer with
        /// default <see cref="TSelf"/> value.
        /// </summary>
        public static virtual Task<bool> WriteAsync<TDevice, TCommunicationInterface>(TDevice device, TDataType requestData,
            CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            TSelf transaction = new TSelf();
            return transaction._WriteAsync<TDevice, TCommunicationInterface>(device, requestData, cancellationToken);
        }
    }
}
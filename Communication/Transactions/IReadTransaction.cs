using IRIS.Communication.Transactions.Abstract;
using IRIS.Devices;

namespace IRIS.Communication.Transactions
{
    /// <summary>
    /// Represents device read request transaction.
    /// </summary>
    public interface IReadTransaction<TSelf, TDataType> : ITransactionWithResponse<TSelf, TDataType>
        where TSelf : unmanaged, IReadTransaction<TSelf, TDataType>
        where TDataType : unmanaged
    {
        /// <summary>
        /// Implement device read transaction logic here.
        /// </summary>
        public Task<TDataType> _ReadAsync<TDevice, TCommunicationInterface>(TDevice device, CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface;
        
        /// <summary>
        /// This method performs read transaction between device and computer with
        /// default <see cref="TSelf"/> value.
        /// </summary>
        public static virtual Task<TDataType> ReadAsync<TDevice, TCommunicationInterface>(TDevice device, CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            TSelf transaction = new TSelf();
            return transaction._ReadAsync<TDevice, TCommunicationInterface>(device, cancellationToken);
        }
    }
}
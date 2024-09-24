using IRIS.Communication.Transactions.Abstract;
using IRIS.Devices;

namespace IRIS.Communication.Transactions
{
    /// <summary>
    /// Represents device read request transaction.
    /// </summary>
    public interface IReadTransaction<TSelf, TResponseDataType> : ITransactionWithResponse<TSelf, TResponseDataType>
        where TSelf : IReadTransaction<TSelf, TResponseDataType>, new()
        where TResponseDataType : struct
    {
        /// <summary>
        /// Implement device read transaction logic here.
        /// </summary>
        public Task<TResponseDataType> _ReadAsync<TDevice, TCommunicationInterface>(TDevice device, CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface;
        
        /// <summary>
        /// This method performs read transaction between device and computer with
        /// default <see cref="TSelf"/> value.
        /// </summary>
        public static virtual Task<TResponseDataType> ReadAsync<TDevice, TCommunicationInterface>(TDevice device, CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            TSelf transaction = new TSelf();
            return transaction._ReadAsync<TDevice, TCommunicationInterface>(device, cancellationToken);
        }
    }
}
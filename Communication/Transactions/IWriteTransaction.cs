using IRIS.Communication.Transactions.Abstract;
using IRIS.Devices;

namespace IRIS.Communication.Transactions
{
    /// <summary>
    /// Represents device write request transaction.
    /// </summary>
    public interface IWriteTransaction<TSelf, in TRequestDataType> : ITransactionWithRequest<TSelf, TRequestDataType>
        where TSelf : IWriteTransaction<TSelf, TRequestDataType>, new()
        where TRequestDataType : struct
    {
        /// <summary>
        /// Implement device write transaction logic here.
        /// </summary>
        public Task<bool> _WriteAsync<TDevice, TCommunicationInterface>(TDevice device, TRequestDataType requestData,
            CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface;
        
        /// <summary>
        /// This method performs write transaction between device and computer with
        /// default <see cref="TSelf"/> value.
        /// </summary>
        public static virtual Task<bool> WriteAsync<TDevice, TCommunicationInterface>(TDevice device, TRequestDataType requestData,
            CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            TSelf transaction = new TSelf();
            return transaction._WriteAsync<TDevice, TCommunicationInterface>(device, requestData, cancellationToken);
        }
    }
}
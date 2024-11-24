﻿using IRIS.Communication;
using IRIS.Devices;
using IRIS.Transactions.Abstract;

namespace IRIS.Transactions
{
    /// <summary>
    /// Represents data exchange transaction between device and computer.
    /// </summary>
    public interface IDataExchangeTransaction<TSelf, in TRequestDataType, TResponseDataType> :
        ITransactionWithRequest<TSelf, TRequestDataType>, ITransactionWithResponse<TSelf, TResponseDataType>
        where TSelf : IDataExchangeTransaction<TSelf, TRequestDataType, TResponseDataType>,
        ITransactionWithRequest<TSelf, TRequestDataType>, ITransactionWithResponse<TSelf, TResponseDataType>, new()
        where TResponseDataType : struct
        where TRequestDataType : struct
    {
        /// <summary>
        /// Implement custom data exchange transaction logic here.
        /// By default, data exchange should write and then read data from device.
        /// </summary>
        public Task<TResponseDataType> _ExchangeAsync<TDevice, TCommunicationInterface>(
            TDevice device,
            TRequestDataType requestData,
            CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface;

        /// <summary>
        /// This method performs data exchange transaction between device and computer with
        /// default <see cref="TSelf"/> value.
        /// </summary>
        public static virtual async Task<TResponseDataType> ExchangeAsync<TDevice, TCommunicationInterface>(
            TDevice device,
            TRequestDataType requestData,
            CancellationToken cancellationToken = default)
            where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            TSelf transaction = new TSelf();
            return await transaction._ExchangeAsync<TDevice, TCommunicationInterface>(device, requestData, cancellationToken);
        }
    }
}
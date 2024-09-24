﻿using IRIS.Communication;
using IRIS.Communication.Transactions;
using IRIS.Communication.Transactions.Abstract;

namespace IRIS.Devices.Transactions
{
    /// <summary>
    /// Represents that device supports specific data exchange transaction.
    /// </summary>
    public interface ISupportExchangeTransaction<TDevice, TCommunicationInterface,
        in TExchangeTransactionType, in TRequestType, TResponseDataType>
        where TExchangeTransactionType : unmanaged,
        IDataExchangeTransaction<TExchangeTransactionType, TRequestType, TResponseDataType>,
        ITransactionWithRequest<TExchangeTransactionType, TRequestType>,
        ITransactionWithResponse<TExchangeTransactionType, TResponseDataType>
        where TRequestType : unmanaged
        where TResponseDataType : unmanaged
        where TDevice : DeviceBase<TCommunicationInterface>
        where TCommunicationInterface : ICommunicationInterface
    {
        /// <summary>
        /// Exchanges data with device.
        /// </summary>
        public async Task<TResponseDataType> ExchangeAsync(TRequestType requestData,
            CancellationToken cancellationToken = default)
        {
            return await TExchangeTransactionType.ExchangeAsync<TDevice, TCommunicationInterface>((TDevice) this,
                requestData, cancellationToken);
        }
        
        /// <summary>
        /// Exchanges data with device using specific transaction.
        /// </summary>
        public async Task<TResponseDataType> ExchangeAsync(TExchangeTransactionType transaction, TRequestType requestData,
            CancellationToken cancellationToken = default)
        {
            return await transaction._ExchangeAsync<TDevice, TCommunicationInterface>((TDevice) this, requestData,
                cancellationToken);
        }
    }
}
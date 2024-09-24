﻿using IRIS.Communication;
using IRIS.Transactions;
using IRIS.Transactions.Abstract;

namespace IRIS.Devices.Transactions
{
    /// <summary>
    /// Represents that device supports specific write transaction
    /// </summary>
    public interface ISupportWriteTransaction<TDevice, TCommunicationInterface, in TWriteTransactionType, TRequestType>
        where TWriteTransactionType : IWriteTransaction<TWriteTransactionType, TRequestType>,
        ITransactionWithRequest<TWriteTransactionType, TRequestType>, new()
        where TRequestType : struct
        where TDevice : DeviceBase<TCommunicationInterface>
        where TCommunicationInterface : ICommunicationInterface
    {
        /// <summary>
        /// Writes data to device
        /// </summary>
        public async Task<bool> WriteAsync(TRequestType requestData,
            CancellationToken cancellationToken = default) =>
            await TWriteTransactionType.WriteAsync<TDevice, TCommunicationInterface>((TDevice) this, requestData, cancellationToken);
        
        /// <summary>
        /// Writes data to device using specific transaction
        /// </summary>
        public async Task<bool> WriteAsync(TWriteTransactionType transaction, TRequestType requestData,
            CancellationToken cancellationToken = default) =>
            await transaction._WriteAsync<TDevice, TCommunicationInterface>((TDevice) this, requestData, cancellationToken);
        
        
    }
}
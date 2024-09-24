﻿using IRIS.Protocols;
using IRIS.Transactions.Abstract;

namespace IRIS.Communication
{
    /// <summary>
    /// Represents communication interface between device and computer
    /// This can be for example serial port, ethernet, etc.
    /// </summary>
    public interface ICommunicationInterface
    {
        /// <summary>
        /// Connect to physical device
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect from physical device
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send data to device
        /// </summary>
        Task SendDataAsync<TProtocol, TTransactionType, TWriteDataType>(TTransactionType transaction,
            TWriteDataType data,
            CancellationToken cancellationToken = default)
            where TProtocol : IProtocol
            where TTransactionType : ITransactionWithRequest<TTransactionType, TWriteDataType>
            where TWriteDataType : struct;
        
        Task<TResponseDataType> ReceiveDataAsync<TProtocol, TTransactionType, TResponseDataType>(
            TTransactionType transaction,
            CancellationToken cancellationToken = default)
            where TProtocol : IProtocol
            where TTransactionType : ITransactionWithResponse<TTransactionType, TResponseDataType>
            where TResponseDataType : struct;
    }
}
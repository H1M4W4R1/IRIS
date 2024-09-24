﻿using IRIS.Protocols;

namespace IRIS.Communication.Transactions.Abstract
{
    /// <summary>
    /// Represents transaction with data.
    /// </summary>
    public interface ITransactionWithRequest<TSelf, in TRequestData> : ICommunicationTransaction<TSelf> 
        where TSelf : ICommunicationTransaction<TSelf>
        where TRequestData : unmanaged
    {
        /// <summary>
        /// Encode data using protocol
        /// </summary>
        public static virtual byte[] Encode<TProtocolType>(TRequestData data)
            where TProtocolType : IProtocol => 
            TProtocolType.EncodeData(data);
    }
}
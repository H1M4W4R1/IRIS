using IRIS.Protocols;

namespace IRIS.Communication.Transactions.Abstract
{
    /// <summary>
    /// Represents transaction with data.
    /// </summary>
    public interface ITransactionWithRequest<TSelf, in TRequestData> : ICommunicationTransaction<TSelf> 
        where TSelf : ICommunicationTransaction<TSelf>
        where TRequestData : struct
    {
        /// <summary>
        /// Encode data using protocol
        /// </summary>
        public byte[] Encode<TProtocolType>(TRequestData data)
            where TProtocolType : IProtocol => 
            TProtocolType.EncodeData(data);
    }
}
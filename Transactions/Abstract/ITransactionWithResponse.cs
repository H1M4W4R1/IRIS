using IRIS.Protocols;

namespace IRIS.Transactions.Abstract
{
    /// <summary>
    /// Represents transaction with response.
    /// </summary>
    public interface ITransactionWithResponse<TSelf, TResponseData> : ICommunicationTransaction<TSelf> 
        where TSelf : ICommunicationTransaction<TSelf>
        where TResponseData : struct
    {
        /// <summary>
        /// Decode data using protocol
        /// </summary>
        public bool Decode<TProtocolType>(byte[] inputData, out TResponseData outputData)
            where TProtocolType : IProtocol => 
            TProtocolType.DecodeData(inputData, out outputData);
    }
}
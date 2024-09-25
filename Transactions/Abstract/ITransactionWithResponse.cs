using IRIS.DataEncoders;

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
        /// Decode data using the specified data encoder.
        /// </summary>
        public bool Decode<TDataEncoderType>(byte[] inputData, out TResponseData outputData)
            where TDataEncoderType : IDataEncoder => 
            TDataEncoderType.DecodeData(inputData, out outputData);
    }
}
using IRIS.DataEncoders;

namespace IRIS.Transactions.Abstract
{
    /// <summary>
    /// Represents transaction with data.
    /// </summary>
    public interface ITransactionWithRequest<TSelf, in TRequestData> : ICommunicationTransaction<TSelf> 
        where TSelf : ICommunicationTransaction<TSelf>
        where TRequestData : struct
    {
        /// <summary>
        /// Encode data using the specified data encoder.
        /// </summary>
        public byte[] Encode<TDataEncoderType>(TRequestData data)
            where TDataEncoderType : IDataEncoder => 
            TDataEncoderType.EncodeData(data);
    }
}
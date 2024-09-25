using IRIS.Communication.Types;
using IRIS.Transactions.Abstract;
using IRIS.Transactions.Readers.Abstract;
using IRIS.Transactions.ReadTypes;

namespace IRIS.Transactions.Readers
{
    /// <summary>
    /// Responsible for reading raw data from the communication interface based
    /// on the length of the data
    /// </summary>
    public readonly struct LengthRawDataReader : IRawDataReader
    {
        public async Task<byte[]> PerformRead<TTransactionType>(IRawDataCommunicationInterface communicationInterface,
            TTransactionType transaction, CancellationToken cancellationToken = default) where TTransactionType : ICommunicationTransaction<TTransactionType>
        {
            // Check if transaction is a read by length transaction
            if (transaction is ITransactionReadByLength byLength)
                return await communicationInterface.ReadRawData(byLength.ResponseLength, cancellationToken);
            
            // If transaction is not supported, throw exception
            throw new NotSupportedException("Transaction type is not supported.");
        }
    }
}
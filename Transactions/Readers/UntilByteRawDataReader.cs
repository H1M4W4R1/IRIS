using IRIS.Communication.Types;
using IRIS.Transactions.Abstract;
using IRIS.Transactions.Readers.Abstract;
using IRIS.Transactions.ReadTypes;

namespace IRIS.Transactions.Readers
{
    /// <summary>
    /// Responsible for reading raw data from the communication interface based
    /// on the expected byte - reads all data until the expected byte is received,
    /// also including the expected byte
    /// </summary>
    public readonly struct UntilByteRawDataReader : IRawDataReader
    {
        public Task<byte[]> PerformRead<TTransactionType>(IRawDataCommunicationInterface communicationInterface,
            TTransactionType transaction,
            CancellationToken cancellationToken = default) where TTransactionType : ICommunicationTransaction<TTransactionType>
        {
            // Check if transaction is a read until byte transaction
            if (transaction is ITransactionReadUntilByte untilByte)
                return communicationInterface.ReadRawDataUntil(untilByte.ExpectedByte, cancellationToken);
            
            // If transaction is not supported, throw exception
            throw new NotSupportedException("Transaction type is not supported.");
        }
    }
}
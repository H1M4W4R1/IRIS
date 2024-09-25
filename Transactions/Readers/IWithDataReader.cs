using IRIS.Communication;
using IRIS.Transactions.Abstract;
using IRIS.Transactions.Readers.Abstract;

namespace IRIS.Transactions.Readers
{
    /// <summary>
    /// Represents a transaction that can read data using specified data reader.
    /// For more information, see <see cref="IDataReader{TCommunicationInterface,TDataType}"/>.
    /// </summary>
    public interface IWithDataReader<TReaderType> : IWithDataReader
        where TReaderType : IDataReader, new()
    {
        /// <summary>
        /// Get data reader of specified type. This method returns instance of TReaderType, even if
        /// requested type is different as in case of multiple reader implementation the compiler
        /// will have ambiguity in selecting the correct type and will request to override this
        /// implementation in derived class, interface or structure. <br/>
        /// <i>Therefore, it's safe to assume that TReaderType will be always equal to TDataReader.</i>
        /// </summary>
        IDataReader IWithDataReader.GetDataReader<TDataReader>() => new TReaderType();
    }

    public interface IWithDataReader
    {
        /// <summary>
        /// Get data reader of specified type
        /// </summary>
        protected IDataReader GetDataReader<TDataReader>();
        
        /// <summary>
        /// Read data from communication interface using specified data reader.
        /// </summary>
        /// <exception cref="NotSupportedException">In case of unsupported reader type</exception>
        /// <exception cref="InvalidCastException">In case of type mismatch</exception>
        public async Task<TDataType> ReadDataAsync<TCommunicationInterface, TTransactionType, TDataReader, TDataType>(
            TCommunicationInterface communicationInterface, TTransactionType transaction,
            CancellationToken cancellationToken = default)
            where TCommunicationInterface : ICommunicationInterface
            where TTransactionType : ICommunicationTransaction<TTransactionType>
            where TDataReader : IDataReader<TCommunicationInterface, TDataType>
            where TDataType : notnull
        {
            // Check if reader is supported
            if (GetDataReader<TDataReader>() is TDataReader withReader)
                return await withReader.PerformRead(communicationInterface, transaction, cancellationToken);
            
            // If reader is not supported, throw exception
            throw new NotSupportedException("Reader type is not supported.");
        }
    }
}
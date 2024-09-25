using IRIS.Communication;
using IRIS.Communication.Types;
using IRIS.Transactions.Abstract;

namespace IRIS.Transactions.Readers.Abstract
{
    /// <summary>
    /// Responsible for reading data from the communication interface.
    /// As there may be multiple implementations of end-of-transaction that will vary between
    /// multiple communication interfaces, this interface is used to provide a common way to
    /// easily implement reading data from the communication interface. Combined with <see cref="IWithDataReader{TReaderType}"/>
    /// allows to easily implement automated data reading from the communication interface used by
    /// e.g. <see cref="IRawDataCommunicationInterface"/> which can use <see cref="LengthRawDataReader"/> or
    /// <see cref="UntilByteRawDataReader"/> to read data from the communication interface.
    /// That way we can add new data readers without the need to change the communication interface.
    /// </summary>
    public interface IDataReader<in TCommunicationInterface, TOutputDataType> : IDataReader
        where TCommunicationInterface : ICommunicationInterface
        where TOutputDataType : notnull
    {
        /// <summary>
        /// Perform read operation via the communication interface under the given transaction
        /// </summary>
        public Task<TOutputDataType> PerformRead<TTransactionType>(TCommunicationInterface communicationInterface,
            TTransactionType transaction,
            CancellationToken cancellationToken = default)
            where TTransactionType : ICommunicationTransaction<TTransactionType>;
        
        async Task<TLocalOuputDataType> IDataReader.PerformRead<TTransactionType, TLocalCommunicationInterface, TLocalOuputDataType>(
            TLocalCommunicationInterface communicationInterface, TTransactionType transaction,
            CancellationToken cancellationToken)
        {
            // Ensure that the communication interface is of the correct type
            if (communicationInterface is not TCommunicationInterface validCommunicationInterface)
                throw new InvalidCastException("Communication interface is not of the correct type.");
            
            TOutputDataType result = await PerformRead(validCommunicationInterface, transaction, cancellationToken);
            
            // Ensure that the output data type is of the correct type
            if (result is not TLocalOuputDataType validOutputData)
                throw new InvalidCastException("Output data type is not of the correct type.");
            
            return validOutputData;
        }
    }

    /// <summary>
    /// Do not use this interface directly. Use <see cref="IDataReader{TCommunicationInterface,TOutputDataType}"/> instead.
    /// </summary>
    public interface IDataReader
    {
        /// <summary>
        /// Read data from the communication interface. This method is used as an entry point for
        /// reading data from the communication interface perspective and may throw exceptions if
        /// types are not matching ones provided in the generic interface.
        /// </summary>
        /// <exception cref="InvalidCastException">In case of type mismatch</exception>
        public Task<TOutputDataType> PerformRead<TTransactionType, TCommunicationInterface, TOutputDataType>(
            TCommunicationInterface communicationInterface, TTransactionType transaction,
            CancellationToken cancellationToken = default)
            where TTransactionType : ICommunicationTransaction<TTransactionType>
            where TCommunicationInterface : ICommunicationInterface
            where TOutputDataType : notnull;
        
    }
}
using IRIS.Transactions.Abstract;
using IRIS.Transactions.Readers;
using IRIS.Transactions.Readers.Abstract;

namespace IRIS.Communication.Types
{
    /// <summary>
    /// Represents raw data communication interface, an interface that can
    /// send or receive raw binary data. <br/><br/>
    /// This interface overrides the default communication interface to allow
    /// easy implementation without the requirement to call any interface-based
    /// methods.
    /// </summary>
    public interface IRawDataCommunicationInterface : ICommunicationInterface
    {
        /// <summary>
        /// Transmit data to device
        /// </summary>
        public void TransmitRawData(byte[] data);

        /// <summary>
        /// Read data from device
        /// </summary>
        public Task<byte[]> ReadRawData(int length, CancellationToken cancellationToken);

        /// <summary>
        /// Read data from device until expected byte is found
        /// </summary>
        public Task<byte[]> ReadRawDataUntil(byte expectedByte, CancellationToken cancellationToken);

        async Task<TResponseDataType> ICommunicationInterface.ReceiveDataAsync<TTransactionType, TResponseDataType>(
            TTransactionType transaction,
            CancellationToken cancellationToken)
        {
            // Check if transaction supports data reader, if not throw exception
            if (transaction is not IWithDataReader withDataReader)
                throw new NotSupportedException("Transaction type is not supported");
            
            // Read data using raw data reader
            byte[] data = await withDataReader
                .ReadDataAsync<IRawDataCommunicationInterface, TTransactionType, IRawDataReader, byte[]>(this,
                    transaction, cancellationToken);

            // Decode data
            return await DecodeData<TResponseDataType, TTransactionType>(transaction, data);
        }

        private Task<TResponseDataType> DecodeData<TResponseDataType, TTransactionType>(
            TTransactionType transaction,
            byte[] rawData) where TResponseDataType : struct
        {
            // Check if user expects raw data and convert it if needed
            if (typeof(TResponseDataType) == typeof(byte[]))
                return Task.FromResult((TResponseDataType) Convert.ChangeType(rawData, typeof(TResponseDataType)));

            // Check if transaction supports encoding data to raw data
            if (transaction is not IWithEncoder<byte[]> withEncoder)
                throw new NotSupportedException("Transaction does not support encoder. Cannot decode data.");

            // Decode data using encoder
            withEncoder.Decode(rawData, out TResponseDataType responseData);
            return Task.FromResult(responseData);
        }

        Task ICommunicationInterface.SendDataAsync<TTransactionType, TWriteDataType>(
            TTransactionType transaction,
            TWriteDataType data,
            CancellationToken cancellationToken)
        {
            byte[] encodedData;

            // Check if transaction supports encoder or if data is raw
            if (transaction is IWithEncoder<byte[]> withEncoder)
                encodedData = withEncoder.Encode(data);
            else if (typeof(TWriteDataType) == typeof(byte[]))
                encodedData = (byte[]) Convert.ChangeType(data, typeof(byte[]));
            else
                throw new NotSupportedException("Transaction type is not supported. Cannot encode data.");

            // Get core interface
            IRawDataCommunicationInterface coreInterface = this;

            // Transmit data
            coreInterface.TransmitRawData(encodedData);

            return Task.CompletedTask;
        }
    }
}
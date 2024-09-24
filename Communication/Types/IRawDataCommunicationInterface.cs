using IRIS.Protocols;
using IRIS.Transactions.Abstract;
using IRIS.Transactions.ReadTypes;

namespace IRIS.Communication.Types
{
    /// <summary>
    /// Represents raw data communication interface, an interface that can
    /// send or receive raw binary data.
    /// </summary>
    public interface IRawDataCommunicationInterface
    {
        /// <summary>
        /// Transmit data to device
        /// </summary>
        internal void TransmitRawData(byte[] data);

        /// <summary>
        /// Read data from device
        /// </summary>
        internal Task<byte[]> ReadRawData(int length, CancellationToken cancellationToken);

        /// <summary>
        /// Read data from device until expected byte is found
        /// </summary>
        internal Task<byte[]> ReadRawDataUntil(byte expectedByte, CancellationToken cancellationToken);

        /// <summary>
        /// Default implementation of receiving data for transactions
        /// </summary>
        public async Task<TResponseDataType>
            DefaultReceiveDataAsyncImpl<TProtocol, TTransactionType, TResponseDataType>(
                TTransactionType transaction,
                CancellationToken cancellationToken = default)
            where TProtocol : IProtocol
            where TTransactionType : ITransactionWithResponse<TTransactionType, TResponseDataType>
            where TResponseDataType : struct
        {
            switch (transaction)
            {
                // If transaction is based on response length, read data until it's length
                case ITransactionReadByLength:
                    return await ProcessReceiveByLength<TProtocol, TTransactionType, TResponseDataType>(transaction,
                        cancellationToken);

                // If transaction is based on response terminator, read data until terminator is found
                case ITransactionReadUntilByte:
                    return await ProcessReceiveByTerminator<TProtocol, TTransactionType, TResponseDataType>(transaction,
                        cancellationToken);
                
                // If transaction is not supported, throw exception
                default:
                    throw new NotSupportedException("Transaction type is not supported");
            }
        }

        private async Task<TResponseDataType> ProcessReceiveByLength<TProtocol, TTransactionType, TResponseDataType>(
            TTransactionType transaction,
            CancellationToken cancellationToken = default)
            where TProtocol : IProtocol
            where TTransactionType : ITransactionWithResponse<TTransactionType, TResponseDataType>
            where TResponseDataType : struct
        {
            // Validate transaction type
            // We need to do this to prevent issues with generic types mismatch
            if (transaction is not ITransactionReadByLength byLength)
                throw new NotSupportedException("Transaction type is not supported");

            byte[] data = await ReadRawData(byLength.ResponseLength, cancellationToken);

            // Decode data
            transaction.Decode<TProtocol>(data, out TResponseDataType responseData);
            return responseData;
        }

        private async Task<TResponseDataType>
            ProcessReceiveByTerminator<TProtocol, TTransactionType, TResponseDataType>(
                TTransactionType transaction,
                CancellationToken cancellationToken = default)
            where TProtocol : IProtocol
            where TTransactionType : ITransactionWithResponse<TTransactionType, TResponseDataType>
            where TResponseDataType : struct
        {
            // Validate transaction type
            // We need to do this to prevent issues with generic types mismatch
            if (transaction is not ITransactionReadUntilByte untilByteReceived)
                throw new NotSupportedException("Transaction type is not supported");

            byte[] data = await ReadRawDataUntil(untilByteReceived.ExpectedByte, cancellationToken);

            // Decode data
            transaction.Decode<TProtocol>(data, out TResponseDataType responseData);
            return responseData;
        }

        /// <summary>
        /// Default implementation of sending data for transactions
        /// </summary>
        public Task DefaultSendDataAsyncImpl<TProtocol, TTransactionType, TWriteDataType>(TTransactionType transaction,
            TWriteDataType data,
            CancellationToken cancellationToken = default) where TProtocol : IProtocol
            where TTransactionType : ITransactionWithRequest<TTransactionType, TWriteDataType>
            where TWriteDataType : struct
        {
            // Encode data
            byte[] encodedData = transaction.Encode<TProtocol>(data);

            // Get core interface
            IRawDataCommunicationInterface coreInterface = this;

            // Transmit data
            coreInterface.TransmitRawData(encodedData);

            return Task.CompletedTask;
        }
    }
}
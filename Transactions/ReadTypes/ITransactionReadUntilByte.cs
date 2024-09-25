using IRIS.Transactions.Readers;

namespace IRIS.Transactions.ReadTypes
{
    /// <summary>
    /// Supports reading transaction until byte is received.
    /// </summary>
    public interface ITransactionReadUntilByte : IWithDataReader<UntilByteRawDataReader>
    {
        /// <summary>
        /// Byte to read until.
        /// </summary>
        public byte ExpectedByte { get; }
        
    }
}
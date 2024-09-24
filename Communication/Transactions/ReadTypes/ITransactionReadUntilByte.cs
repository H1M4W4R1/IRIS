namespace IRIS.Communication.Transactions.ReadTypes
{
    /// <summary>
    /// Supports reading transaction until byte is received.
    /// </summary>
    public interface ITransactionReadUntilByte
    {
        /// <summary>
        /// Byte to read until.
        /// </summary>
        public byte ExpectedByte { get; }
        
    }
}
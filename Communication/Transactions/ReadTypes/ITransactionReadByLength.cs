namespace IRIS.Communication.Transactions.ReadTypes
{
    /// <summary>
    /// Supports reading transaction by length.
    /// </summary>
    public interface ITransactionReadByLength
    {
        /// <summary>
        /// Length of the response.
        /// </summary>
        public int ResponseLength { get; }

    }
}
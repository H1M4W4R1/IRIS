using IRIS.Transactions.Readers;

namespace IRIS.Transactions.ReadTypes
{
    /// <summary>
    /// Supports reading transaction by length.
    /// </summary>
    public interface ITransactionReadByLength : IWithDataReader<LengthRawDataReader>
    {
        /// <summary>
        /// Length of the response.
        /// </summary>
        public int ResponseLength { get; }

    }
}
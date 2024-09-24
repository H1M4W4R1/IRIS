using IRIS.Utility;

namespace IRIS.Protocols.LINE.Data
{
    /// <summary>
    /// Represents a read transaction for basic line communication.
    /// Line can have up to 128 characters, other characters will be ignored.
    /// </summary>
    public struct LineTransactionData()
    {
        /// <summary>
        /// Internal data storage.
        /// </summary>
        public UnmanagedString128 data = new();

        /// <summary>
        /// Creates a new instance of LineData with provided text.
        /// </summary>
        public LineTransactionData(string text) : this()
        {
            data.CopyFrom(text);
        }

        /// <summary>
        /// Gets the text stored in this instance.
        /// </summary>
        public override string ToString() => data.ToString();
    }
}
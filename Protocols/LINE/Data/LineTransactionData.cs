namespace IRIS.Protocols.LINE.Data
{
    /// <summary>
    /// Represents a read transaction for basic line communication.
    /// Line can have up to 128 characters, other characters will be ignored.
    /// </summary>
    public readonly struct LineTransactionData(string msg)
    {
        /// <summary>
        /// Internal data storage.
        /// </summary>
        public readonly string message = msg;

        /// <summary>
        /// Gets the text stored in this instance.
        /// </summary>
        public override string ToString() => message;
    }
}
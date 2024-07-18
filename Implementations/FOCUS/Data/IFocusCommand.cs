namespace IRIS.Implementations.FOCUS.Data
{
    /// <summary>
    /// Represents command for FOCUS protocol
    /// FOCUS commands are requested by sending request bytes and waiting for response
    /// FOCUS commands and responses are fixed length
    /// Length of response is determined by command, however error response is always 4 bytes long
    /// (1 byte for error code, 2 bytes for error message and ENDL byte)    
    /// </summary>
    public interface IFocusCommand
    {
        /// <summary>
        /// Bytes to send to device when requesting command
        /// </summary>
        public byte[] RequestBytes { get; }
        
        /// <summary>
        /// Amount of bytes to read from device for full response
        /// Including status byte and ENDL byte
        /// </summary>
        public int ResponseLength { get; }
        
        /// <summary>
        /// Decode response data to struct for this command
        /// </summary>
        public TData Decode<TData>(byte[] data) where TData : struct;
    }
}
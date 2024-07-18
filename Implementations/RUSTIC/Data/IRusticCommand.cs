namespace IRIS.Implementations.RUSTIC.Data
{
    /// <summary>
    /// Represents command for RUSTIC protocol
    /// which is ASCII-based protocol that is used for communication with devices. <br/><br/>
    /// It sends commands via simple assignments
    /// If value is assigned to question mark then it means that device should return value
    /// aka. <see cref="RusticCommandType.Get"/>
    /// Otherwise, if value is assigned to something else, it means that device should set value
    /// aka. <see cref="RusticCommandType.Set"/>
    /// </summary>
    public interface IRusticCommand
    {
        /// <summary>
        /// Identifier of command
        /// </summary>
        public string Identifier { get; }
        
        /// <summary>
        /// Type of command
        /// </summary>
        public RusticCommandType CommandType { get; set; }
        
        /// <summary>
        /// Decode response from device
        /// </summary>
        public TDataObject Decode<TDataObject>(byte[] data) where TDataObject : struct;

        /// <summary>
        /// Encode object to ASCII string data
        /// </summary>
        public string Encode();
    }
}
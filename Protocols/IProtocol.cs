namespace IRIS.Protocols
{
    /// <summary>
    /// Represents communication protocol between device and computer. Protocol is an interface used to encode or decode
    /// transactions from/to binary data. <br/>
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// Encode provided data to binary format
        /// </summary>
        public static abstract byte[] EncodeData<TData>(TData inputData) where TData : struct;
        
        /// <summary>
        /// Decode provided binary data to type TData, where TData is unmanaged structure
        /// </summary>
        public static abstract bool DecodeData<TData>(byte[] inputData, out TData outputData) where TData : struct;
    }
}
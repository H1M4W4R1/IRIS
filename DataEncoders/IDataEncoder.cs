namespace IRIS.DataEncoders
{
    /// <summary>
    /// Represents communication interface that can encode and decode data for
    /// transactions from/to binary data. <br/>
    /// </summary>
    public interface IDataEncoder
    {
        /// <summary>
        /// Encode provided data to binary format
        /// </summary>
        public static abstract byte[] EncodeData<TData>(TData inputData) where TData : struct;
        
        /// <summary>
        /// Decode provided binary data to type TData, where TData is structure
        /// </summary>
        public static abstract bool DecodeData<TData>(byte[] inputData, out TData outputData) where TData : struct;
    }
}
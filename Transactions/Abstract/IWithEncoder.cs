using IRIS.DataEncoders;

namespace IRIS.Transactions.Abstract
{
    /// <summary>
    /// Represents object that supports specific encoder.
    /// Encoders are not used on data, but on objects that use that data.
    /// </summary>
    public interface IWithEncoder<TEncoderType> : IWithEncoder
        where TEncoderType : IDataEncoder
    {
        byte[] IWithEncoder.Encode<TRequestData>(TRequestData data)
            where TRequestData : struct =>
            (byte[]) TEncoderType.EncodeData(data);

        bool IWithEncoder.Decode<TResponseData>(byte[] data, out TResponseData result)
            where TResponseData : struct =>
            TEncoderType.DecodeData<TResponseData>(data, out result);
    }

    public interface IWithEncoder
    {
        /// <summary>
        /// Encode data using this encoder.
        /// </summary>
        public byte[] Encode<TRequestData>(TRequestData data)
            where TRequestData : struct;

        /// <summary>
        /// Decode data using this encoder.
        /// </summary>
        public bool Decode<TResponseData>(byte[] data, out TResponseData result)
            where TResponseData : struct;
    }
}
using IRIS.DataEncoders;

namespace IRIS.Transactions.Abstract
{
    /// <summary>
    /// Represents object that supports specific encoder.
    /// Encoders are not used on data, but on objects that use that data.
    /// </summary>
    /// <typeparam name="TEncoderType">Type of encoder to use</typeparam>
    /// <typeparam name="TEncoderTargetData">Type of data that encoder can encode/decode</typeparam>
    public interface IWithEncoder<TEncoderType, TEncoderTargetData> : IWithEncoder<TEncoderTargetData>
        where TEncoderType : IDataEncoder
        where TEncoderTargetData : notnull
    {
        TEncoderTargetData IWithEncoder<TEncoderTargetData>.Encode<TRequestData>(TRequestData data)
            where TRequestData : struct => TEncoderType.EncodeData<TRequestData, TEncoderTargetData>(data);

        bool IWithEncoder<TEncoderTargetData>.Decode<TResponseData>(TEncoderTargetData data, out TResponseData result)
            where TResponseData : struct =>
            TEncoderType.DecodeData(data, out result);
    }

    public interface IWithEncoder<TEncoderTargetData>
    {
        /// <summary>
        /// Encode data using this encoder.
        /// </summary>
        public TEncoderTargetData Encode<TRequestData>(TRequestData data)
            where TRequestData : struct;

        /// <summary>
        /// Decode data using this encoder.
        /// </summary>
        public bool Decode<TResponseData>(TEncoderTargetData data, out TResponseData result)
            where TResponseData : struct;
    }
}
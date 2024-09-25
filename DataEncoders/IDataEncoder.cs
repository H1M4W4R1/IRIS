namespace IRIS.DataEncoders
{
    /// <summary>
    /// Represents communication interface that can encode and decode data for
    /// transactions from/to specified data type. 
    /// </summary>
    public interface IDataEncoder<TSelf, TRelateDataType> : IDataEncoder
        where TSelf : IDataEncoder<TSelf, TRelateDataType>
        where TRelateDataType : notnull
    {
        /// <summary>
        /// Encode provided data to specified format
        /// </summary>
        public static abstract TRelateDataType _EncodeData<TData>(TData inputData) where TData : struct;

        /// <summary>
        /// Decode provided specified data to type TData, where TData is structure
        /// </summary>
        public static abstract bool _DecodeData<TData>(TRelateDataType inputData, out TData outputData)
            where TData : struct;

        static object IDataEncoder.EncodeData<TData>(TData inputData) =>
            TSelf._EncodeData(inputData);

        static bool IDataEncoder.DecodeData<TData>(object inputData, out TData outputData) =>
            TSelf._DecodeData((TRelateDataType) inputData, out outputData);

        static TLocalRelateDataType IDataEncoder.EncodeData<TData, TLocalRelateDataType>(TData inputData)
        {
            // Check if TRelateDataType1 is TRelateDataType
            if (typeof(TLocalRelateDataType) != typeof(TRelateDataType))
                throw new NotSupportedException("This feature is not supported for this encoder");

            // Call the actual method
            TRelateDataType result = TSelf._EncodeData(inputData);

            // Convert the obtained response to the correct type
            return (TLocalRelateDataType) Convert.ChangeType(result, typeof(TLocalRelateDataType))!;
        }

        static bool IDataEncoder.DecodeData<TData, TLocalRelateDataType>(TLocalRelateDataType inputData,
            out TData outputData)
        {
            // Check if TRelateDataType1 is TRelateDataType
            if (typeof(TLocalRelateDataType) != typeof(TRelateDataType))
                throw new NotSupportedException("This feature is not supported for this encoder");

            // Call the actual method
            return TSelf._DecodeData((TRelateDataType) Convert.ChangeType(inputData, typeof(TRelateDataType)),
                out outputData);
        }
    }

    /// <summary>
    /// This is a marker interface for data encoders. Do not use this interface directly.
    /// </summary>
    public interface IDataEncoder
    {
        /// <summary>
        /// Encode provided data to specified format
        /// </summary>
        public static abstract TRelateDataType EncodeData<TData, TRelateDataType>(TData inputData)
            where TData : struct
            where TRelateDataType : notnull;

        /// <summary>
        /// Decode provided specified data to type TData, where TData is structure
        /// </summary>
        public static abstract bool DecodeData<TData, TRelateDataType>(TRelateDataType inputData, out TData outputData)
            where TData : struct
            where TRelateDataType : notnull;

        /// <summary>
        /// Encode provided data to specified format
        /// </summary>
        public static abstract object EncodeData<TData>(TData inputData) where TData : struct;

        /// <summary>
        /// Decode provided specified data to type TData, where TData is structure
        /// </summary>
        public static abstract bool DecodeData<TData>(object inputData, out TData outputData) where TData : struct;
    }
}
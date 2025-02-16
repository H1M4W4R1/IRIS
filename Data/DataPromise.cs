using System.Diagnostics.CodeAnalysis;

namespace IRIS.Data
{
    /// <summary>
    /// Promise for data from device
    /// </summary>
    /// <param name="isSuccess">True if the promise is successful</param>
    /// <param name="data">Data received from device</param>
    /// <typeparam name="TDataType">Type of data received from device</typeparam>
    public readonly struct DataPromise<TDataType>(bool isSuccess, TDataType? data)
    {
        /// <summary>
        /// True if the promise is successful
        /// </summary>
        public bool IsSuccess { get; } = isSuccess;

        /// <summary>
        /// Data received from device
        /// </summary>
        public TDataType? Data { get; } = data;
        
        /// <summary>
        /// Check if promise has data and is successful
        /// </summary>
        [MemberNotNullWhen(true, nameof(Data))]
        public bool HasData => Data is not null && IsSuccess;
        
        public static DataPromise<TDataType> FromSuccess(TDataType? data) => new(true, data);
        public static DataPromise<TDataType> FromFailure(TDataType? data = default) => new(false, data);
    }
    
    /// <summary>
    /// Proxy layer for <see cref="DataPromise{TDataType}"/>
    /// </summary>
    public static class DataPromise
    {
        public static DataPromise<TDataType> FromSuccess<TDataType>(TDataType? data) => DataPromise<TDataType>.FromSuccess(data);
        public static DataPromise<TDataType> FromFailure<TDataType>(TDataType? data = default) => DataPromise<TDataType>.FromFailure(data);
    }
}
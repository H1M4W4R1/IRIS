using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace IRIS.Operations.Abstract
{
    /// <summary>
    ///     Represents the result of a device operation that includes data.
    /// </summary>
    /// <typeparam name="TDataType">Type of data included in the result.</typeparam>
    public interface IDeviceOperationResult<out TDataType> : IDeviceOperationResult
    {
        /// <summary>
        ///     Data included in the result.
        /// </summary>
        TDataType? Data { get; }

        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        /// <remarks>
        ///     This aggressively overrides and reverse-proxies the <see cref="IDeviceOperationResult.IsSuccess" />
        ///     property to allow usage of MemberNotNullWhen attribute for better syntax suggestions.
        /// </remarks>
        [MemberNotNullWhen(true, nameof(Data))]
        public new bool IsSuccess { get; }
        
        bool IDeviceOperationResult.IsSuccess => IsSuccess;
    }

    /// <summary>
    ///     Represents the result of a device operation.
    /// </summary>
    public interface IDeviceOperationResult
    {
        /// <summary>
        ///     Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        ///     Check if the result is operation with data of the specified type.
        /// </summary>
        /// <remarks>
        ///     If you provide different data type than operation was performed with
        ///     this method will return false.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasData<TDataType>() => this is IDeviceOperationResult<TDataType>;
    }
}
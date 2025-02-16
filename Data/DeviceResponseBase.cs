using IRIS.Data.Implementations;

namespace IRIS.Data
{
    /// <summary>
    /// Represents response from device with data
    /// </summary>
    /// <param name="data">Data received from device</param>
    /// <typeparam name="TDataType">Type of data received from device</typeparam>
    public abstract class DeviceResponseBase<TDataType>(TDataType data) : DeviceResponseBase
    {
        /// <summary>
        /// Data received from device
        /// </summary>
        public TDataType Data { get; set; } = data;
    }
    
    /// <summary>
    /// Represents response from device
    /// </summary>
    public abstract class DeviceResponseBase
    {
        /// <summary>
        /// Check if response has data
        /// </summary>
        /// <typeparam name="TDataType">Type of data</typeparam>
        /// <returns>True if response has data, false otherwise</returns>
        public bool HasData<TDataType>() => this is DeviceResponseBase<TDataType>;
        
        /// <summary>
        /// Check if response is OK
        /// </summary>
        public bool IsOK => this is OKResponse;
        
        /// <summary>
        /// Check if response is error
        /// </summary>
        public bool IsError => this is ErrorResponse;

        /// <summary>
        /// Get data from response
        /// </summary>
        /// <typeparam name="TData">Type of data</typeparam>
        /// <returns>Data from response or null if response has no data</returns>
        public TData? GetData<TData>()
        {
            if (this is DeviceResponseBase<TData> response) return response.Data;
            return default;
        }
    }
}
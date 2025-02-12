using Windows.Devices.Bluetooth.Advertisement;

namespace IRIS.Addressing.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth LE service address
    /// Can also match device name using regular expression
    /// </summary>
    public readonly struct BluetoothLEServiceAddress : IBluetoothLEAddress
    {
        /// <summary>
        /// Cached advertisement filter to avoid creating it every time
        /// </summary>
        private readonly BluetoothLEAdvertisementFilter _cachedFilter;
        
        /// <summary>
        /// Regular expression to match device name
        /// </summary>
        public string? NameRegex { get; init; }
        
        /// <summary>
        /// UUID of the service
        /// </summary>
        public Guid ServiceUUID { get; init; }

        /// <summary>
        /// Get the advertisement filter for this service
        /// </summary>
        public BluetoothLEAdvertisementFilter GetAdvertisementFilter() => _cachedFilter;

        /// <summary>
        /// Constructor
        /// </summary>
        public BluetoothLEServiceAddress(Guid serviceUuid)
        {
            ServiceUUID = serviceUuid;
            _cachedFilter = new BluetoothLEAdvertisementFilter()
            {
                Advertisement = new BluetoothLEAdvertisement
                {
                    ServiceUuids =
                    {
                        ServiceUUID
                    }
                }
            };
        }

    }
}
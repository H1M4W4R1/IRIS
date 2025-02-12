using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace IRIS.Addressing.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth LE service address
    /// Can also match device name using regular expression
    /// </summary>
    public readonly struct BluetoothLEServiceAddress : IBluetoothLEAddress
    {
        private readonly BluetoothLEAdvertisementFilter _cachedAdvertisementFilter;
        private readonly BluetoothSignalStrengthFilter _cachedSignalStrengthFilter;

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
        public BluetoothLEAdvertisementFilter GetAdvertisementFilter() => _cachedAdvertisementFilter;

        /// <summary>
        /// Get the signal strength filter for this service
        /// </summary>
        public BluetoothSignalStrengthFilter GetSignalStrengthFilter() => _cachedSignalStrengthFilter;

        /// <summary>
        /// Check if the device is valid for this service
        /// </summary>
        public async Task<bool> IsDeviceValid(BluetoothLEDevice device)
        {
            // Cache service UUID because C#...
            Guid uuid = ServiceUUID;
            
            // Get service
            GattDeviceServicesResult serviceResult = await device.GetGattServicesAsync();

            // Ensure communication status is OK
            if (serviceResult.Status != GattCommunicationStatus.Success) return false;

            // Get service
            GattDeviceService? service = 
                serviceResult.Services.FirstOrDefault(s => s.Uuid == uuid);

            // Check if device has the service
            return service != null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BluetoothLEServiceAddress(
            Guid serviceUUID,
            short minSignalStrength = -75,
            short maxSignalStrength = -70,
            uint timeoutSeconds = 2)
        {
            // Copy service UUIDs
            ServiceUUID = serviceUUID;

            // Create advertisement filter
            _cachedAdvertisementFilter = new BluetoothLEAdvertisementFilter
            {
                Advertisement = new BluetoothLEAdvertisement
                {
                    ServiceUuids =
                    {
                        ServiceUUID
                    }
                }
            };

            // Create signal filter
            _cachedSignalStrengthFilter = new BluetoothSignalStrengthFilter
            {
                InRangeThresholdInDBm = maxSignalStrength,
                OutOfRangeThresholdInDBm = minSignalStrength,
                OutOfRangeTimeout = TimeSpan.FromSeconds(timeoutSeconds),
            };
        }
    }
}
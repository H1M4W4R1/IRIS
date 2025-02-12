using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace IRIS.Addressing.Bluetooth
{
    /// <summary>
    /// Bluetooth device address
    /// </summary>
    public interface IBluetoothLEAddress
    {
        /// <summary>
        /// Get the advertisement filter for this address
        /// </summary>
        public BluetoothLEAdvertisementFilter GetAdvertisementFilter();
        
        /// <summary>
        /// Get the signal strength filter for this address
        /// </summary>
        public BluetoothSignalStrengthFilter GetSignalStrengthFilter();

        /// <summary>
        /// Check if the device is valid for this address
        /// </summary>
        public Task<bool> IsDeviceValid(BluetoothLEDevice device);
    }
}
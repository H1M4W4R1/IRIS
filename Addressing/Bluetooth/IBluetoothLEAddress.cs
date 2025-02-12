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
    }
}
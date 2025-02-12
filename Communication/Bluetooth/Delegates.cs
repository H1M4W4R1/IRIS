using Windows.Devices.Bluetooth;

namespace IRIS.Communication.Bluetooth
{
    /// <summary>
    /// Called when device connection is lost
    /// </summary>
    public delegate void DeviceDisconnected(ulong address, BluetoothLEDevice device);

    /// <summary>
    /// Called when a device is connected
    /// </summary>
    public delegate void DeviceConnected(ulong address, BluetoothLEDevice device);
}
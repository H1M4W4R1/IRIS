using IRIS.Addressing.Bluetooth;
using IRIS.Communication.Bluetooth;

namespace IRIS.Devices
{
    /// <summary>
    /// Represents a Bluetooth LE device
    /// </summary>
    public abstract class BluetoothLEDevice : DeviceBase<BluetoothLEInterface>
    {
        public BluetoothLEDevice(string deviceNameRegex)
        {
            HardwareAccess = new BluetoothLEInterface(deviceNameRegex);
        }
        
        public BluetoothLEDevice(Guid serviceUUID)
        {
            HardwareAccess = new BluetoothLEInterface(serviceUUID);
        }
        
    }
}
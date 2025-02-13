using IRIS.Communication.Bluetooth;
using IRIS.Devices.Abstract;

namespace IRIS.Devices.Bluetooth
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
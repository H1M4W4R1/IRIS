using IRIS.Communication.Bluetooth;
using IRIS.Devices.Abstract;

namespace IRIS.Devices.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth LE device
    /// </summary>
    public abstract class BluetoothLEDeviceBase : DeviceBase<BluetoothLEInterface>
    {
        public BluetoothLEDeviceBase(string deviceNameRegex)
        {
            HardwareAccess = new BluetoothLEInterface(deviceNameRegex);
        }
        
        public BluetoothLEDeviceBase(Guid serviceUUID)
        {
            HardwareAccess = new BluetoothLEInterface(serviceUUID);
        }
    }
}
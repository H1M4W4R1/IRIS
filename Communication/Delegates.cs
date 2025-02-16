namespace IRIS.Communication
{
    // TODO: Attach events to interface to be required in all devices
    public class Delegates
    {
        public delegate void OnDeviceConnected<in TDeviceAddress>(TDeviceAddress address);
        public delegate void OnDeviceDisconnected<in TDeviceAddress>(TDeviceAddress address);
        public delegate void DeviceConnectionLost<in TDeviceAddress>(TDeviceAddress address);
    }
}
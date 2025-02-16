namespace IRIS.Communication
{
    // TODO: Attach events to interface to be required in all devices
    public static class Delegates
    {
        public delegate void DeviceConnectedHandler<in TDeviceAddress>(TDeviceAddress address);
        public delegate void DeviceDisconnectedHandler<in TDeviceAddress>(TDeviceAddress address);
        public delegate void DeviceConnectionLostHandler<in TDeviceAddress>(TDeviceAddress address);
    }
}
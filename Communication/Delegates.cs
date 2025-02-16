namespace IRIS.Communication
{
    public static class Delegates
    {
        public delegate void DeviceConnectedHandler<in TDeviceAddress>(TDeviceAddress address);
        public delegate void DeviceDisconnectedHandler<in TDeviceAddress>(TDeviceAddress address);
        public delegate void DeviceConnectionLostHandler<in TDeviceAddress>(TDeviceAddress address);
    }
}
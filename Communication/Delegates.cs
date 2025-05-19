namespace IRIS.Communication
{
    /// <summary>
    /// Contains delegate definitions for device connection-related events.
    /// These delegates are used to handle various device connection states and events
    /// in the communication system.
    /// </summary>
    public static class Delegates
    {
        /// <summary>
        /// Represents a method that handles device connection events.
        /// </summary>
        /// <typeparam name="TDeviceAddress">The type of address used to identify the device.</typeparam>
        /// <param name="address">The address of the device that has connected.</param>
        public delegate void DeviceConnectedHandler<in TDeviceAddress>(TDeviceAddress address);

        /// <summary>
        /// Represents a method that handles device disconnection events.
        /// </summary>
        /// <typeparam name="TDeviceAddress">The type of address used to identify the device.</typeparam>
        /// <param name="address">The address of the device that has disconnected.</param>
        public delegate void DeviceDisconnectedHandler<in TDeviceAddress>(TDeviceAddress address);

        /// <summary>
        /// Represents a method that handles device connection loss events.
        /// This is typically used when a connection is unexpectedly terminated
        /// rather than through a normal disconnection process.
        /// </summary>
        /// <typeparam name="TDeviceAddress">The type of address used to identify the device.</typeparam>
        /// <param name="address">The address of the device that has lost connection.</param>
        public delegate void DeviceConnectionLostHandler<in TDeviceAddress>(TDeviceAddress address);
    }
}
namespace IRIS.Recognition
{
    /// <summary>
    /// Represents a method that handles device addition events in the system.
    /// </summary>
    /// <typeparam name="THardwareAddress">The type representing the hardware address of the device.</typeparam>
    /// <typeparam name="TSoftwareAddress">The type representing the software address of the device.</typeparam>
    /// <param name="hardwareDevice">The hardware address of the device that was added.</param>
    /// <param name="softwareDevice">The software address of the device that was added.</param>
    public delegate void DeviceAddedEventHandler<in THardwareAddress, in TSoftwareAddress>(THardwareAddress hardwareDevice, TSoftwareAddress softwareDevice);
    
    /// <summary>
    /// Represents a method that handles device removal events in the system.
    /// </summary>
    /// <typeparam name="THardwareAddress">The type representing the hardware address of the device.</typeparam>
    /// <typeparam name="TSoftwareAddress">The type representing the software address of the device.</typeparam>
    /// <param name="hardwareDevice">The hardware address of the device that was removed.</param>
    /// <param name="softwareDevice">The software address of the device that was removed.</param>
    public delegate void DeviceRemovedEventHandler<in THardwareAddress, in TSoftwareAddress>(THardwareAddress hardwareDevice, TSoftwareAddress softwareDevice);
}
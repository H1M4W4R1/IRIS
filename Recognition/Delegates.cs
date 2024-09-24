namespace IRIS.Recognition
{
    /// <summary>
    /// Called when a device is added to the system.
    /// </summary>
    public delegate void DeviceAddedEventHandler<in THardwareAddress, in TSoftwareAddress>(THardwareAddress hardwareDevice, TSoftwareAddress softwareDevice);
    
    /// <summary>
    /// Called when a device is removed from the system.
    /// </summary>
    public delegate void DeviceRemovedEventHandler<in THardwareAddress, in TSoftwareAddress>(THardwareAddress hardwareDevice, TSoftwareAddress softwareDevice);
}
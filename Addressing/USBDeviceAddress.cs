namespace IRIS.Addressing
{
    /// <summary>
    /// USB Device Address - used to store addresses of devices connected via USB
    /// Contains Vendor ID and Product ID of the device (VID and PID)
    /// </summary>
    /// <remarks>
    /// This type assumes that there is only one device with the same VID and PID connected to the system,
    /// so in some cases it may not be sufficient to uniquely identify the device.
    /// </remarks>
    public readonly struct USBDeviceAddress(string vid, string pid) : IDeviceAddress
    {
        /// <summary>
        /// Vendor ID of the device.
        /// </summary>
        public readonly string VID { get; } = vid;

        /// <summary>
        /// Product ID of the device.
        /// </summary>
        public readonly string PID { get; } = pid;
    }
}
using IRIS.Addressing.Abstract;

namespace IRIS.Addressing.Ports
{
    /// <summary>
    /// Represents a device address for USB-connected devices using Vendor ID (VID) and Product ID (PID).
    /// This implementation allows devices to be uniquely identified by their USB identifiers.
    /// </summary>
    /// <remarks>
    /// The address is composed of two components:
    /// - Vendor ID (VID): A unique identifier assigned to the device manufacturer
    /// - Product ID (PID): A unique identifier assigned to the specific product by the manufacturer
    /// 
    /// Note: This implementation assumes that there is only one device with the same VID and PID
    /// connected to the system. In scenarios where multiple identical devices are connected,
    /// additional identification methods may be required.
    /// </remarks>
    public readonly struct USBDeviceAddress(string vid, string pid) : IDeviceAddress
    {
        /// <summary>
        /// Gets the Vendor ID of the USB device.
        /// </summary>
        /// <value>
        /// A string representing the manufacturer's unique identifier.
        /// </value>
        public readonly string VID { get; } = vid;

        /// <summary>
        /// Gets the Product ID of the USB device.
        /// </summary>
        /// <value>
        /// A string representing the product's unique identifier within the manufacturer's catalog.
        /// </value>
        public readonly string PID { get; } = pid;

        /// <summary>
        /// Returns a string representation of the USB device address in the format "VID:PID".
        /// </summary>
        /// <returns>
        /// A string containing the concatenated VID and PID values, separated by a colon.
        /// </returns>
        public override string ToString() => $"{VID}:{PID}";
        
        /// <summary>
        /// Attempts to parse a string into a USBDeviceAddress instance.
        /// </summary>
        /// <param name="address">The string to parse, expected in the format "VID:PID".</param>
        /// <returns>
        /// A USBDeviceAddress instance if parsing is successful; otherwise, null.
        /// </returns>
        /// <remarks>
        /// The input string must contain exactly one colon (:) separating the VID and PID values.
        /// Any other format will result in a null return value.
        /// </remarks>
        public static USBDeviceAddress? Parse(string address)
        {
            string[] splitAddress = address.Split(':');
            
            // Check if the address is valid
            if (splitAddress.Length != 2)
                return null;
            
            return new USBDeviceAddress(splitAddress[0], splitAddress[1]);
        }
    }
}
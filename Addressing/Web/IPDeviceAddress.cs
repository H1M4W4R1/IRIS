using System.Net;
using IRIS.Addressing.Abstract;

namespace IRIS.Addressing.Web
{
    /// <summary>
    /// Represents a device address using Internet Protocol (IP) addressing.
    /// This implementation allows devices to be accessed using their IP address,
    /// supporting both IPv4 and IPv6 formats.
    /// </summary>
    /// <remarks>
    /// The address is stored as an IPAddress object, which can represent both IPv4 and IPv6 addresses.
    /// This provides a standardized way to reference devices on IP-based networks such as Ethernet or WiFi.
    /// </remarks>
    public readonly struct IPDeviceAddress(IPAddress ipAddress) : IDeviceAddress<IPAddress>
    {
        /// <summary>
        /// Gets the IP address value used to identify the device.
        /// </summary>
        /// <value>
        /// An IPAddress object representing the device's network address.
        /// </value>
        public IPAddress Address { get; } = ipAddress;
        
        /// <summary>
        /// Gets an IPDeviceAddress representing the local loopback address (127.0.0.1).
        /// </summary>
        /// <value>
        /// An IPDeviceAddress instance configured with the loopback address.
        /// </value>
        public static IPDeviceAddress Localhost => new IPDeviceAddress(IPAddress.Loopback);
        
        /// <summary>
        /// Gets an IPDeviceAddress representing the local network interface (0.0.0.0).
        /// </summary>
        /// <value>
        /// An IPDeviceAddress instance configured with the local network address.
        /// </value>
        public static IPDeviceAddress Local => new IPDeviceAddress(IPAddress.Any);
        
        /// <summary>
        /// Gets an IPDeviceAddress representing the broadcast address (255.255.255.255).
        /// </summary>
        /// <value>
        /// An IPDeviceAddress instance configured with the broadcast address.
        /// </value>
        public static IPDeviceAddress Broadcast => new IPDeviceAddress(IPAddress.Broadcast);
        
        /// <summary>
        /// Parses a string representation of an IP address into an IPDeviceAddress instance.
        /// </summary>
        /// <param name="address">The string to parse, expected to be a valid IP address format.</param>
        /// <returns>
        /// An IPDeviceAddress instance containing the parsed IP address.
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown when the input string is not in a valid IP address format.
        /// </exception>
        public static IPDeviceAddress Parse(string address) => new IPDeviceAddress(IPAddress.Parse(address));
        
        /// <summary>
        /// Returns a string representation of the device's IP address.
        /// </summary>
        /// <returns>
        /// A string containing the IP address in standard format.
        /// </returns>
        public override string ToString() => Address.ToString();
    }
}
using System.Net;
using IRIS.Addressing.Abstract;

namespace IRIS.Addressing.Web
{
    /// <summary>
    ///     Internet Protocol (IP) device address
    ///     Used to store addresses of devices connected via IP (Ethernet, WiFi)
    /// </summary>
    public readonly struct IPDeviceAddress(IPAddress ipAddress) : IDeviceAddress<IPAddress>
    {
        /// <summary>
        ///     Address of the device
        /// </summary>
        public IPAddress Address { get; } = ipAddress;
        
        public static IPDeviceAddress Localhost => new IPDeviceAddress(IPAddress.Loopback);
        
        public static IPDeviceAddress Local => new IPDeviceAddress(IPAddress.Any);
        
        public static IPDeviceAddress Broadcast => new IPDeviceAddress(IPAddress.Broadcast);
        
        /// <summary>
        ///     Parse IP address from provided string.
        /// </summary>
        /// <param name="address">IP address in string format.</param>
        /// <returns>Parsed IPDeviceAddress object.</returns>
        /// <exception cref="FormatException">Thrown when the address is not in the correct format.</exception>
        public static IPDeviceAddress Parse(string address) => new IPDeviceAddress(IPAddress.Parse(address));
        
        public override string ToString() => Address.ToString();
    }
}
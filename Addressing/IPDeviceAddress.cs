using System.Net;

namespace IRIS.Addressing
{
    /// <summary>
    /// Internet Protocol (IP) device address
    /// Used to store addresses of devices connected via IP (Ethernet, WiFi)
    /// </summary>
    public readonly struct IPDeviceAddress(IPAddress ipAddress)
    {
        /// <summary>
        /// Address of the device
        /// </summary>
        public IPAddress Address { get; } = ipAddress;
        
        public static IPDeviceAddress Localhost => new IPDeviceAddress(IPAddress.Loopback);
        
        public static IPDeviceAddress Local => new IPDeviceAddress(IPAddress.Any);
        
        public static IPDeviceAddress Broadcast => new IPDeviceAddress(IPAddress.Broadcast);
        
        public static IPDeviceAddress Parse(string address) => new IPDeviceAddress(IPAddress.Parse(address));
    }
}
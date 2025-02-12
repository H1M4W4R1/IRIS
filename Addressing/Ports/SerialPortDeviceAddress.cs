using System.Runtime.InteropServices;
using IRIS.Addressing.Abstract;

namespace IRIS.Addressing.Ports
{
    /// <summary>
    /// Struct representing serial port address
    /// Used to store addresses of devices connected via serial port
    /// Example: COM9 in Windows or /dev/ttyUSB0 in Linux
    /// </summary>
    public readonly struct SerialPortDeviceAddress : IDeviceAddress<string>, IEquatable<SerialPortDeviceAddress>
    {
        /// <summary>
        /// COM Port Name (for example COM9)
        /// </summary>
        public string Address { get; }
        
        /// <summary>
        /// Get address for Linux USB port
        /// </summary>
        public static SerialPortDeviceAddress LinuxUSB(int portIndex) => 
            new SerialPortDeviceAddress($"/dev/ttyUSB{portIndex}");
        
        /// <summary>
        /// Get address for Linux ACM port
        /// </summary>
        public static SerialPortDeviceAddress LinuxACM(int portIndex) =>
            new SerialPortDeviceAddress($"/dev/ttyACM{portIndex}");
        
        /// <summary>
        /// Get address for Windows COM port
        /// </summary>
        public static SerialPortDeviceAddress Windows(int portIndex) =>
            new SerialPortDeviceAddress($"COM{portIndex}");

        /// <summary>
        /// Create Serial Port Device Address using
        /// </summary>
        /// <param name="deviceAddress">Index of the device aka. port number</param>
        /// <exception cref="PlatformNotSupportedException">When platform is not supported</exception>
        /// <remarks>Not supported on OSX nor BSD</remarks>
        public SerialPortDeviceAddress(int deviceAddress)
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Address = $"COM{deviceAddress}";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Try to connect via USB
                Address = $"/dev/ttyUSB{deviceAddress}";
                
                // Check if Linux USB port exists, if not try to connect via ACM
                if (!File.Exists(Address))
                    Address = $"/dev/ttyACM{deviceAddress}";
                
                // Check if Linux ACM port exists, if not try to connect via AMA
                if (!File.Exists(Address))
                    Address = $"/dev/ttyAMA{deviceAddress}";
                
                // Check if Linux ACM port exists, if not try to connect via SAC
                if (!File.Exists(Address))
                    Address = $"/dev/ttySAC{deviceAddress}";
                
                // Check if Linux ACM port exists, if not try regular COM/RS232 port
                if (!File.Exists(Address))
                    Address = $"/dev/ttyS{deviceAddress}";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new PlatformNotSupportedException("OSX port number uses weird naming for USB and thus is not supported via numeric index");
            }
            else
                throw new PlatformNotSupportedException();
        }
        
        /// <summary>
        /// Create Serial Port Device Address using
        /// fully-qualified device address, for example COM9 or /dev/ttyUSB0. <br/>
        /// Fully supports Windows, Linux and OSX.
        /// </summary>
        /// <param name="deviceAddress">Fully-qualified device address</param>
        public SerialPortDeviceAddress(string deviceAddress) 
        {
            // Check if platform is Linux
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Check if file name starts with /dev
                if (deviceAddress.StartsWith("/dev"))
                    Address = deviceAddress;
                else if(deviceAddress.StartsWith("dev"))
                    Address = $"/{deviceAddress}";
                else
                    Address = $"/dev/{deviceAddress}";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Check if file name starts with COM
                if (deviceAddress.StartsWith("COM"))
                    Address = deviceAddress;
                // Attempt to fix COM port name if case is wrong
                else if(deviceAddress.StartsWith("com"))
                    Address = $"COM{deviceAddress.Replace("com", "")}";
                // Attempt to connect to COM port if only number is provided
                else
                    Address = $"COM{deviceAddress}";
            }
            else
            {
                // OSX and other platforms require full path
                Address = deviceAddress;
            }
        }
        
        public static bool operator ==(SerialPortDeviceAddress left, SerialPortDeviceAddress right) => left.Address == right.Address;
        public static bool operator !=(SerialPortDeviceAddress left, SerialPortDeviceAddress right) => !(left == right);
        
        public bool Equals(SerialPortDeviceAddress other) => Address == other.Address;
        public override bool Equals(object? obj) => obj is SerialPortDeviceAddress other && Equals(other);

        public override int GetHashCode() => Address.GetHashCode();
    }
}

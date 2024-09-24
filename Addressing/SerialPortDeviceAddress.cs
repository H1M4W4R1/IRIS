namespace IRIS.Addressing
{
    /// <summary>
    /// Struct representing serial port address
    /// Used to store addresses of devices connected via serial port
    /// Example: COM9 in Windows or /dev/ttyUSB0 in Linux
    /// </summary>
    public readonly struct SerialPortDeviceAddress(string address) : IDeviceAddress<string>
    {
        /// <summary>
        /// COM Port Name (for example COM9)
        /// </summary>
        public string Address { get; } = address;
        
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
        
    }
}

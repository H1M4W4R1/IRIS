using System.IO.Ports;

namespace IRIS.Communication.Serial.Settings
{
    /// <summary>
    /// Configuration of serial interface
    /// </summary>
    public readonly struct SerialInterfaceSettings(int baudRate = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
    {
        /// <summary>
        /// Baud rate of serial port
        /// Default: 115200
        /// </summary>
        public readonly int baudRate = baudRate;
        
        /// <summary>
        /// Parity of serial port
        /// Default: None
        /// </summary>
        public readonly Parity parity = parity;
        
        /// <summary>
        /// Data bits of serial port
        /// Default: 8
        /// </summary>
        public readonly int dataBits = dataBits;
        
        /// <summary>
        /// Stop bits of serial port
        /// Default: One
        /// </summary>
        public readonly StopBits stopBits = stopBits;
    }
}
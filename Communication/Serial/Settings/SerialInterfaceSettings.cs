using System.IO.Ports;

namespace IRIS.Communication.Serial.Settings
{
    /// <summary>
    /// Configuration of serial interface
    /// </summary>
    public readonly struct SerialInterfaceSettings(int baudRate = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One,
        bool useDtr = false, bool useRts = false)
    {
        // ReSharper disable once InconsistentNaming
        public static readonly SerialInterfaceSettings Default = new(115200, Parity.None, 8, StopBits.One, false, false);
        
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
        
        /// <summary>
        /// Determines whether the Data Terminal Ready (DTR) signal is enabled during serial communication.
        /// Default: false
        /// </summary>
        public readonly bool dtrEnable = useDtr;

        /// <summary>
        /// Determines whether the Request to Send (RTS) signal is enabled during serial communication.
        /// Default: false
        /// </summary>
        public readonly bool rtsEnable = useRts;
    }
}
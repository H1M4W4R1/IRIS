namespace IRIS.Communication
{
    /// <summary>
    /// Represents communication interface between device and computer
    /// This can be for example serial port, ethernet, etc.
    /// </summary>
    public interface ICommunicationInterface
    {
        /// <summary>
        /// Current data length in queue
        /// </summary>
        int DataLength { get; }
        
        /// <summary>
        /// Connect to physical device
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect from physical device
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Transmit data to device.
        /// </summary>
        void TransmitData(params byte[] data);

        /// <summary>
        /// Read data from device 
        /// </summary>
        byte[] ReadData(int length);
        
        /// <summary>
        /// Peek data from device 
        /// </summary>
        byte[] PeekData(int length);

        /// <summary>
        /// Read data from device until specific byte is received
        /// </summary>
        byte[] ReadDataUntil(byte receivedByte);

        /// <summary>
        /// Check if byte is in data queue
        /// </summary>
        bool HasByte(byte character);
    }
}
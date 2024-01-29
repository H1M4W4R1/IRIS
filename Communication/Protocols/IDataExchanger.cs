namespace IRIS.Communication.Protocols
{
    /// <summary>
    /// Data exchanger is used to transmit and receive data from embedded device.
    /// </summary>
    public interface IDataExchanger
    {

        /// <summary>
        /// Connect to physical device
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect from physical device
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Transmit data to device
        /// </summary>
        void TransmitData(byte[] data);

        /// <summary>
        /// Receive data from device (and remove from queue)
        /// </summary>
        byte[] ReceiveData(int length);

        /// <summary>
        /// Receive data until got specified byte
        /// </summary>
        byte[] ReceiveDataUntil(byte receivedByte);

        /// <summary>
        /// Peek received data (does not remove data from queue)
        /// </summary>
        byte[] PeekReceivedData(int length);

        /// <summary>
        /// Check if data has specified byte
        /// </summary>
        bool HasByte(byte b);

        /// <summary>
        /// Get current data length
        /// </summary>
        int GetLength();

        /// <summary>
        /// Check if data exchanger is connected to device
        /// </summary>
        bool IsConnected();
    }
}

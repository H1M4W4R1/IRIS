namespace IRIS.Exceptions
{
    public abstract class IRISExceptionBase(string message) : Exception(message)
    {
        /// <summary>
        /// Converts exception to <see cref="ValueTask{TResult}"/>
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <returns>ValueTask with exception</returns>
        public ValueTask<TResult> ToValueTask<TResult>() => ValueTask.FromException<TResult>(this);
    }
    
#region DATA_EXCEPTIONS

    
    
    public abstract class DataException(string message) : IRISExceptionBase(message);

    /// <summary>
    /// Raised when not enough data is stored in buffer or received
    /// </summary>
    public sealed class NotEnoughDataException(int found, int expected)
        : DataException($"Not enough data. Found {found} bytes, expected {expected} bytes");
    
    /// <summary>
    /// Raised when too much data is stored in buffer or received
    /// </summary>
    public sealed class DataOverflowException(int found, int expected)
        : DataException($"Data overflow. Found {found} bytes, expected {expected} bytes");
    
    /// <summary>
    /// Raised when timeout occurs
    /// </summary>
    public sealed class ResponseTimeoutException()
        : DataException($"Response timeout.");

    public abstract class InvalidDataException(string message)
        : DataException($"Invalid data. {message}");
    
    public sealed class InvalidDataDoesNotContainByte(byte expected)
        : InvalidDataException($"Data does not contain expected byte {expected:X2}");
    
#endregion

    public abstract class DeviceException(string message) : IRISExceptionBase(message);

    public sealed class DeviceAlreadyDisconnectedException() : DeviceException("Device is already disconnected");

    public sealed class DeviceAlreadyConnectedException() : DeviceException("Device is already connected");

    public class DeviceNotFoundException() : DeviceException("Device not found");

    public class DeviceNotConnectedException() : DeviceException("Device not connected");

    public class DeviceConnectionLostException() : DeviceException("Device connection lost");

    public class DeviceConnectionFailedException() : DeviceException("Device connection failed");

    public class DeviceConnectionTimeoutException() : DeviceException("Device connection timeout");
    
    public class DeviceTransmissionFailed() : DeviceException("Device transmission failed due to unknown error");

    public abstract class HardwareException(string message) : IRISExceptionBase(message);

    public abstract class ProtocolException(string message) : IRISExceptionBase(message);
}
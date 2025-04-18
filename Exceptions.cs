namespace IRIS
{
    public class CommunicationFailedException(string message) : Exception(message);
    public class NotEnoughDataException(string data) : Exception(data);
    public class ValidDataNotFoundException(string data) : Exception(data);
    public class DeviceNotConnectedException(string message) : Exception(message);
    public class DeviceNotFoundException(string message) : Exception(message);
    public class DeviceNotRespondingException(string message) : Exception(message);
    public class DeviceNotSupportedException(string message) : Exception(message);
    public class DeviceAlreadyConnectedException(string message) : Exception(message);
    public class DeviceAlreadyDisconnectedException(string message) : Exception(message);
    public class DeviceAlreadyInUseException(string message) : Exception(message);
    public class DeviceNotInitializedException(string message) : Exception(message);
    public class DeviceNotConfiguredException(string message) : Exception(message);
    public class DeviceNotReadyException(string message) : Exception(message);
    
}
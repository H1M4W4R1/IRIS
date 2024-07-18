namespace IRIS
{
    /// <summary>
    /// Exception thrown when communication with device fails
    /// </summary>
    public class CommunicationException(string msg) : Exception(msg);
    
    /// <summary>
    /// Exception thrown when execution of command fails
    /// </summary>
    public class ExecutionException(string msg) : Exception(msg);
    
    /// <summary>
    /// Exception thrown when hardware error occurs
    /// </summary>
    public class HardwareException(string msg) : Exception(msg);
}

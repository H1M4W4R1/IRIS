namespace IRIS
{
    /// <summary>
    /// Exception thrown when communication with device fails
    /// </summary>
    public sealed class CommunicationException(string msg) : Exception(msg);
    
    /// <summary>
    /// Exception thrown when execution of command fails
    /// </summary>
    public sealed class ExecutionException(string msg) : Exception(msg);
    
    /// <summary>
    /// Exception thrown when hardware error occurs
    /// </summary>
    public sealed class HardwareException(string msg) : Exception(msg);
}

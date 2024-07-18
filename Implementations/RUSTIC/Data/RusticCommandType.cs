namespace IRIS.Implementations.RUSTIC.Data
{
    /// <summary>
    /// Type of RUSTIC command
    /// It should be either GET or SET
    /// Otherwise, it is invalid command, so no modification should be done to this enumeration
    /// </summary>
    public enum RusticCommandType
    {
        Get,
        Set
    }
}
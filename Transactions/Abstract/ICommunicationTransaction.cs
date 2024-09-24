namespace IRIS.Transactions.Abstract
{
    /// <summary>
    /// Represents data transaction between device and computer.
    /// This can be for example serial port, ethernet, etc.
    /// <br/>
    /// Communication transactions should be unmanaged structs (this is intended to reduce memory allocation).
    /// </summary>
    public interface ICommunicationTransaction<TSelf> : ICommunicationTransaction
        where TSelf : ICommunicationTransaction<TSelf>
    {

    }

    /// <summary>
    /// Represents internal data transaction between device and computer.
    /// This type is intended to remove requirement for generic to be used in method definitions.
    /// </summary>
    public interface ICommunicationTransaction
    {
        
    }
}
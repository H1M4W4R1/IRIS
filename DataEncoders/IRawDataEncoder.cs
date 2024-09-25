namespace IRIS.DataEncoders
{
    /// <summary>
    /// Represents communication interface that can encode and decode data for
    /// transactions from/to binary data. <br/>
    /// </summary>
    public interface IRawDataEncoder<TSelf> : IDataEncoder<TSelf, byte[]>
        where TSelf : IRawDataEncoder<TSelf>
    {
        
    }
}
using IRIS.Communication.Types;

namespace IRIS.Transactions.Readers.Abstract
{
    /// <summary>
    /// Data reader for raw data
    /// </summary>
    public interface IRawDataReader : IDataReader<IRawDataCommunicationInterface, byte[]>
    {
        
    }
}
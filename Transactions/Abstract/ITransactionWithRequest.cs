namespace IRIS.Transactions.Abstract
{
    /// <summary>
    /// Represents transaction with data.
    /// </summary>
    public interface ITransactionWithRequest<TSelf, in TRequestData> : ICommunicationTransaction<TSelf> 
        where TSelf : ICommunicationTransaction<TSelf>
        where TRequestData : struct
    {

    }
}
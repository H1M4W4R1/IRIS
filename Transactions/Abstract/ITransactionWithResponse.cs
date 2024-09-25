namespace IRIS.Transactions.Abstract
{
    /// <summary>
    /// Represents transaction with response data
    /// </summary>
    public interface ITransactionWithResponse<TSelf, TResponseData> : ICommunicationTransaction<TSelf> 
        where TSelf : ICommunicationTransaction<TSelf>
        where TResponseData : struct
    {

    }
}
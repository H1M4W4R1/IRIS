using IRIS.Communication.Transactions.ReadTypes;

namespace IRIS.Protocols.LINE.Abstract
{
    public interface ILineTransaction : ITransactionReadUntilByte
    {
        byte ITransactionReadUntilByte.ExpectedByte => 0x0A;

    }
}
using IRIS.Transactions.ReadTypes;

namespace IRIS.DataEncoders.LINE.Abstract
{
    public interface ILineTransaction : ITransactionReadUntilByte
    {
        byte ITransactionReadUntilByte.ExpectedByte => 0x0A;

    }
}
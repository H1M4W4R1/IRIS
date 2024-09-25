using IRIS.Transactions.Abstract;
using IRIS.Transactions.ReadTypes;

namespace IRIS.DataEncoders.LINE.Abstract
{
    public interface ILineTransaction : ITransactionReadUntilByte, IWithEncoder<LineDataEncoder, byte[]>
    {
        byte ITransactionReadUntilByte.ExpectedByte => 0x0A;

    }
}
using IRIS.Transactions.Abstract;
using IRIS.Transactions.ReadTypes;

namespace IRIS.DataEncoders.RUSTIC.Abstract
{
    public interface IRusticTransaction : ITransactionReadUntilByte, IWithEncoder<RusticDataEncoder, byte[]>
    {
        byte ITransactionReadUntilByte.ExpectedByte => 0x0A;
    }
}
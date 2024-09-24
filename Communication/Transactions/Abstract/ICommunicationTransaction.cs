using IRIS.Communication.Transactions.ReadTypes;

namespace IRIS.Communication.Transactions.Abstract
{
    /// <summary>
    /// Represents data transaction between device and computer.
    /// This can be for example serial port, ethernet, etc.
    /// <br/>
    /// Communication transactions must be structs (this is intended to reduce memory allocation). <br/>
    /// We also assume that all transactions are unmanaged, as most devices will be using C or C++ for internal
    /// software and most types should be compatible with unmanaged C# types. <br/>
    /// The issues may arise at arrays or similar types, however we can create custom type for handling that.
    /// </summary>
    public interface ICommunicationTransaction<TSelf> : ICommunicationTransaction
        where TSelf : ICommunicationTransaction<TSelf>
    {
        // ReSharper disable once StaticMemberInGenericType
        public static virtual int ResponseLength => 0;

        // ReSharper disable once StaticMemberInGenericType
        public static virtual byte ExpectedByte => 0x0A;

        public static virtual bool IsByLength
        {
            get
            {
                Type[] interfaces = typeof(TSelf).GetInterfaces();
                
                // Check if implements ITransactionReadByLength
                for (int index = 0; index < interfaces.Length; index++)
                {
                    Type interfaceType = interfaces[index];
                    if (interfaceType == typeof(ITransactionReadByLength)) return true;
                }

                return false;
            }
        }

        public static virtual bool IsByEndingByte
        {
            get
            {
                Type[] interfaces = typeof(TSelf).GetInterfaces();
                
                // Check if implements ITransactionReadByLength
                for (int index = 0; index < interfaces.Length; index++)
                {
                    Type interfaceType = interfaces[index];
                    if (interfaceType == typeof(ITransactionReadUntilByte)) return true;
                }

                return false;
            }
        }
   
    }

    /// <summary>
    /// Represents internal data transaction between device and computer.
    /// This type is intended to remove requirement for generic to be used in method definitions.
    /// </summary>
    public interface ICommunicationTransaction
    {
        
    }
}
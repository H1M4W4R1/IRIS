using IRIS.Communication.Protocols.Recognition;

namespace IRIS.Communication.Protocols
{
    public interface IProtocol<TSelf> : IProtocol
    {
        /// <summary>
        /// Check if command is valid for current protocol (if meets protocol specs)
        /// </summary>
        // bool IsValidCommand(byte[] array); // TODO: remove this

        /// <summary>
        /// Register data exchanger to communicate with device
        /// </summary>
        TSelf UsesDataExchanger(IDataExchanger dataExchanger);

        /// <summary>
        /// Validate protocol
        /// </summary>
        TSelf Validate();

        /// <summary>
        /// Mark this protocol as finalized
        /// </summary>
        TSelf Build();
    }

    public interface IProtocol
    {
        IDataExchanger? GetDataExchanger();
    }
}

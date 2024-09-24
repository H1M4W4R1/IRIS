using IRIS.Addressing;
using IRIS.Devices;
using IRIS.Devices.Interfaces;
using IRIS.Devices.Interfaces.Settings;
using IRIS.Devices.Transactions;
using IRIS.Protocols.LINE.Data;
using IRIS.Protocols.LINE.Transactions;

namespace IRIS.Protocols.LINE.Examples
{
    /// <summary>
    /// This serial device is used for simple communication with LINE protocol. <br/>
    /// You can test it with any simple echo software on your e.g. Arduino.
    /// </summary>
    public abstract class SimpleSerialCommunicationDevice(
        SerialPortDeviceAddress deviceAddress,
        SerialInterfaceSettings settings)
        : SerialDeviceBase(deviceAddress, settings),
            ISupportExchangeTransaction<SimpleSerialCommunicationDevice, SerialPortInterface, LineExchangeTransaction,
                LineTransactionData, LineTransactionData>,
            ISupportReadTransaction<SimpleSerialCommunicationDevice, SerialPortInterface, LineReadTransaction,
                LineTransactionData>,
            ISupportWriteTransaction<SimpleSerialCommunicationDevice, SerialPortInterface, LineWriteTransaction,
                LineTransactionData>;
}
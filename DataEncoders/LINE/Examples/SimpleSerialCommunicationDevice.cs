using IRIS.Addressing;
using IRIS.Communication.Serial.Settings;
using IRIS.Devices;

namespace IRIS.DataEncoders.LINE.Examples
{
    /// <summary>
    /// This serial device is used for simple communication with LINE protocol. <br/>
    /// You can test it with any simple echo software on your e.g. Arduino.
    /// </summary>
    public abstract class SimpleSerialCommunicationDevice(SerialPortDeviceAddress deviceAddress,
        SerialInterfaceSettings settings) : SerialDeviceBase(deviceAddress, settings);
}
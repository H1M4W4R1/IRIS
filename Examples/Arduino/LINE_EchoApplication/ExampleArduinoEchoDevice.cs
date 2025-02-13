using IRIS.Addressing;
using IRIS.Addressing.Ports;
using IRIS.Communication.Serial.Settings;
using IRIS.Implementations.Serial;

namespace IRIS.Examples.Arduino.LINE_EchoApplication
{
    /// <summary>
    /// This class represents an example device that can exchange messages with an Arduino device.
    /// </summary>
    public sealed class ExampleArduinoEchoDevice(SerialPortDeviceAddress deviceAddress, SerialInterfaceSettings settings) : LINEDeviceBase(deviceAddress, settings)
    {
        
    }
}
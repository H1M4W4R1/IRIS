using IRIS.Addressing;
using IRIS.Addressing.Ports;
using IRIS.Communication.Serial.Settings;

namespace IRIS.Examples.Arduino.LINE_EchoApplication
{
    public static class ExampleApp
    {
        public static async void RunApp(string comPort)
        {
            // Create new Arduino echo device
            ExampleArduinoEchoDevice device =
                new(new SerialPortDeviceAddress(comPort), new SerialInterfaceSettings(115200));
            device.Connect();

            // Exchange data example
            string response = await device.ExchangeMessages("Hello, Arduino!\r\n");
            Console.Write(response);

            // Send message example
            await device.SendMessage("This API is really simple!\r\n");
        
            // Read message example
            string message = await device.ReadMessage();
            Console.Write(message);

            device.Disconnect();
        }
    }
}
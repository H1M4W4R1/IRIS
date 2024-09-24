using IRIS.Addressing;
using IRIS.Devices.Interfaces.Settings;

namespace IRIS.Examples.ArduinoRusticLED
{
    public static class ExampleApp
    {
        public static async void RunApp(string comPort)
        {
            // Create new Arduino echo device
            ExampleArduinoLEDChangingDevice device =
                new(new SerialPortDeviceAddress(comPort), new SerialInterfaceSettings(115200));
            device.Connect();

            // Exchange data example
            bool ledValue = await device.GetLEDValue();
            Console.WriteLine("LED value: " + ledValue);

            // Wait for 500 ms to see the result
            await Task.Delay(500);
            
            // Set LED value
            bool setOk = await device.SetLEDValue(true);
            Console.WriteLine("Set LED value was success: " + setOk);
            
            // Read LED value
            ledValue = await device.GetLEDValue();
            Console.WriteLine("LED value after set: " + ledValue);
            
            // Wait for 500 ms to see the result
            await Task.Delay(500);
            
            // Set LED value
            setOk = await device.SetLEDValue(false);
            Console.WriteLine("Set LED value was success: " + setOk);
            
            // Read LED value
            ledValue = await device.GetLEDValue();
            Console.WriteLine("LED value after set: " + ledValue);
            
            // Disconnect device
            device.Disconnect();
        }
    }
}
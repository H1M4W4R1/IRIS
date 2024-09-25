using IRIS.Addressing;
using IRIS.Recognition;

namespace IRIS.Examples.Recognition
{
    public static class ExampleUSBRecognitionApp
    {
        private static WindowsUSBSerialPortDeviceWatcher _deviceWatcher = default!;
        
        public static async void RunApp()
        {
            // Create new USB recognition watcher
            _deviceWatcher = new WindowsUSBSerialPortDeviceWatcher();
            
            // Attach event handler
            _deviceWatcher.OnDeviceAdded += OnDeviceAdded;
            _deviceWatcher.OnDeviceRemoved += OnDeviceRemoved;
            
            // Start watching for USB devices
            _deviceWatcher.Start();
        }

        public static async void KillApp()
        {
            // Stop watching for USB devices
            _deviceWatcher.Stop();
        }

        private static void OnDeviceRemoved(USBDeviceAddress hardwareDevice, SerialPortDeviceAddress softwareDevice)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Device disconnected: {hardwareDevice.VID}:{hardwareDevice.PID} [{softwareDevice.Address}]");
            Console.ResetColor();
        }

        private static void OnDeviceAdded(USBDeviceAddress hardwareDevice, SerialPortDeviceAddress softwareDevice)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Device connected: {hardwareDevice.VID}:{hardwareDevice.PID} [{softwareDevice.Address}]");
            Console.ResetColor();
        }
    }
}
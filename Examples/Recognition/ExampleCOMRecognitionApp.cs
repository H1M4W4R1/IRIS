using IRIS.Addressing;
using IRIS.Recognition;

namespace IRIS.Examples.Recognition
{
    public static class ExampleCOMRecognitionApp 
    {
        private static SerialPortDeviceWatcher _deviceWatcher = default!;
        
        public static async void RunApp()
        {
            // Create new COM recognition watcher
            _deviceWatcher = new SerialPortDeviceWatcher();
            
            // Attach event handler
            _deviceWatcher.OnDeviceAdded += OnDeviceAdded;
            _deviceWatcher.OnDeviceRemoved += OnDeviceRemoved;
            
            // Start watching for COM devices
            _deviceWatcher.Start();
        }
        
        public static async void KillApp()
        {
            // Stop watching for COM devices
            _deviceWatcher.Stop();
        }
        
        private static void OnDeviceRemoved(SerialPortDeviceAddress hardwareDevice, SerialPortDeviceAddress softwareDevice)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Device disconnected: {softwareDevice.Address}");
            Console.ResetColor();
        }
        
        private static void OnDeviceAdded(SerialPortDeviceAddress hardwareDevice, SerialPortDeviceAddress softwareDevice)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Device connected: {softwareDevice.Address}");
            Console.ResetColor();
        }
        
    }
}
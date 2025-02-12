using System.IO.Ports;
using IRIS.Addressing;
using IRIS.Addressing.Ports;

namespace IRIS.Recognition
{
    /// <summary>
    /// Watcher used to check for serial port devices. This class is used to scan for all available serial ports
    /// via <see cref="SerialPort.GetPortNames"/>. It does not handle checking for USB device VID/PID and thus
    /// lists all available serial ports, even if the device might not be supported. <br/>
    /// It is strongly advices not to use this class as it may lead to unexpected behavior in many cases.
    /// For better results use <see cref="WindowsUSBSerialPortDeviceWatcher"/> instead.
    /// </summary>
    public sealed class SerialPortDeviceWatcher : DeviceWatcherBase<SerialPortDeviceWatcher, SerialPortDeviceAddress>
    {
        /// <summary>
        /// Scan for all available devices
        /// </summary>
        protected override Task<(List<SerialPortDeviceAddress>, List<SerialPortDeviceAddress>)>
            ScanForDevicesAsync(CancellationToken cancellationToken)
        {
            // Get all available serial ports
            string[] ports = SerialPort.GetPortNames();
            
            // Create lists for hardware and software devices
            List<SerialPortDeviceAddress> hardwareDevices = new();
            List<SerialPortDeviceAddress> softwareDevices = new();
            
            // Loop through all ports
            for (int serialPortIndex = 0; serialPortIndex < ports.Length; serialPortIndex++)
            {
                string port = ports[serialPortIndex];
                // Create device address
                SerialPortDeviceAddress deviceAddress = new(port);

                // Add device to list
                hardwareDevices.Add(deviceAddress);
                softwareDevices.Add(deviceAddress);
            }
            
            // Return devices
            return Task.FromResult((hardwareDevices, softwareDevices));
        }
    }
}
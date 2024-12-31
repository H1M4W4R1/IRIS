using System.Management;
using System.Runtime.InteropServices;
using IRIS.Addressing;
using Microsoft.Win32;

namespace IRIS.Recognition
{
    /// <summary>
    /// This class is used to scan for all available USB serial port devices.
    /// It validates the devices by checking the VID and PID of the device, so you can easily check if the device
    /// is supported.
    /// </summary>
    public sealed class WindowsUSBSerialPortDeviceWatcher(USBDeviceAddress? hwAddress = null)
        : DeviceWatcherBase<WindowsUSBSerialPortDeviceWatcher, USBDeviceAddress, SerialPortDeviceAddress>
    {
        /// <summary>
        /// Hardware address of the device expected to be connected.
        /// If null, all CDC devices will be returned.
        /// </summary>
        private USBDeviceAddress? HardwareAddress { get; } = hwAddress;
        
        /// <summary>
        /// Scan for all available devices
        /// </summary>
        protected override Task<(List<USBDeviceAddress>, List<SerialPortDeviceAddress>)>
            ScanForDevicesAsync(CancellationToken cancellationToken)
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException("This class is only supported on Windows platform.");
            
            // Create lists for hardware and software devices
            List<USBDeviceAddress> hardwareDevices = [];
            List<SerialPortDeviceAddress> softwareDevices = [];
            
            // Create entity
            using ManagementClass registerAccessEntity = new("Win32_PnPEntity");
       
            // Registry path
            const string localMachineSystemCurrentControlSet = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\";

            // Loop through all instances
            foreach (ManagementBaseObject? o in registerAccessEntity.GetInstances())
            {
                ManagementObject? registryObject = (ManagementObject) o;
                object? registryGUID = registryObject.GetPropertyValue("ClassGuid");
                if (registryGUID == null || registryGUID.ToString()?.ToUpper() != "{4D36E978-E325-11CE-BFC1-08002BE10318}")
                    continue; // Skip all devices except device class "PORTS"

                string? deviceID = registryObject.GetPropertyValue("PnpDeviceID").ToString();
                string registryDeviceEnumerationDataPath = localMachineSystemCurrentControlSet + "Enum\\" + deviceID + "\\Device Parameters";
                string? portName = Registry.GetValue(registryDeviceEnumerationDataPath, "PortName", "")?.ToString();

                // Check if ID exists
                if (deviceID == null) continue;

                // Skip if not a port
                if (portName == null) continue;

                // Parse Device ID
                string[] parsedId = deviceID.Split("\\", StringSplitOptions.RemoveEmptyEntries);
                if (parsedId.Length <= 0) continue;

                // Check if is usb
                if (parsedId[0] != "USB") continue;

                // Split VID and PID
                string[] vendorAndProductIdentifiers = parsedId[1].Split("&");
                if (vendorAndProductIdentifiers.Length < 2) // Some devices have shitty names with more parameters
                    continue;

                // Remove trash data
                string vid1 = vendorAndProductIdentifiers[0].Replace("VID_", "");
                string pid1 = vendorAndProductIdentifiers[1].Replace("PID_", "");

                // Check if hardware address is set, if not return all CDC devices connected
                if (HardwareAddress == null)
                {
                    // Create serial port address
                    SerialPortDeviceAddress deviceAddress = new(portName);
                    USBDeviceAddress hardwareAddress = new(vid1, pid1);
                    
                    // Add device to list
                    hardwareDevices.Add(hardwareAddress);
                    softwareDevices.Add(deviceAddress);
                }
                else
                {
                    // Check if device is meeting VID and PID criteria
                    if (vid1 != HardwareAddress.Value.VID || pid1 != HardwareAddress.Value.PID) continue;
                    
                    // Create serial port address
                    SerialPortDeviceAddress deviceAddress = new(portName);
                    
                    // Add device to list
                    hardwareDevices.Add(HardwareAddress.Value);
                    softwareDevices.Add(deviceAddress);
                }
            }
            
            // Return devices
            return Task.FromResult((hardwareDevices, softwareDevices));
        }
        
    }
}
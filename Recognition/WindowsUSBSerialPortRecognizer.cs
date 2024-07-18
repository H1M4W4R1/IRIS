using System.Management;
using System.Runtime.InteropServices;
using IRIS.Addressing;
using Microsoft.Win32;

namespace IRIS.Recognition
{
    public readonly struct WindowsUSBSerialPortRecognizer(string vid, string pid) : IDeviceRecognizer<SerialPortDeviceAddress>
    {
        /// <summary>
        /// Checks if a device meets the specified VID and PID criteria.
        /// </summary>
        /// <param name="deviceDeviceAddress">The address of the device to check.</param>
        /// <returns>True if the device meets the criteria, false otherwise.</returns>
        /// <remarks>
        /// This method checks if the device is a USB device and if it's VID and PID match the specified criteria.
        /// It uses the Win32_PnPEntity class to access the registry and retrieve information about the device.
        /// It only supports Windows platform.
        /// </remarks>
        public bool CheckDevice(SerialPortDeviceAddress deviceDeviceAddress)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return false;
      
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
                if (vendorAndProductIdentifiers.Length != 2)
                    throw new InvalidDataException("Cannot parse VID/PID");

                // Remove trash data
                string vid1 = vendorAndProductIdentifiers[0].Replace("VID_", "");
                string pid1 = vendorAndProductIdentifiers[1].Replace("PID_", "");

                // Check if device is meeting VID and PID criteria
                if (vid1 == vid && pid1 == pid)
                    return true;
            }
            
            return false;
        }

        /// <summary>
        /// Scans for devices that meet the specified VID and PID criteria.
        /// </summary>
        /// <returns>A list of addresses for devices that meet the specified VID and PID criteria.</returns>
        /// <remarks>
        /// This method uses the Win32_PnPEntity class to access the Registry and retrieve information about the devices.
        /// It only supports Windows platform.
        /// It loops through all instances of the Win32_PnPEntity class, checking each device's VID and PID.
        /// If a device's VID and PID match the specified criteria, its address is added to the list.
        /// The method returns the list of addresses for devices that meet the specified VID and PID criteria.
        /// </remarks>
        public List<SerialPortDeviceAddress> ScanForDevices()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return [];
            
            // Create list of addresses
            List<SerialPortDeviceAddress> addressList = [];

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
                if (vendorAndProductIdentifiers.Length != 2)
                    throw new InvalidDataException("Cannot parse VID/PID");

                // Remove trash data
                string vid1 = vendorAndProductIdentifiers[0].Replace("VID_", "");
                string pid1 = vendorAndProductIdentifiers[1].Replace("PID_", "");

                // Check if device is meeting VID and PID criteria and add serial port address to list
                if (vid1 == vid && pid1 == pid)
                    addressList.Add(new SerialPortDeviceAddress(portName));
            }

            return addressList;
        }
    }
}

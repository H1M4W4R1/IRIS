using IRIS.Communication.Protocols.Addressing;
using IRIS.Exceptions;
using Microsoft.Win32;
using System.Management;
using System.Runtime.InteropServices;


namespace IRIS.Communication.Protocols.Recognition
{
    public class USBSerialPortRecognizer : IDeviceRecognizer
    {
        private string _vid;
        private string _pid;

        public bool CheckCriteria(DeviceAddress p)
        {
            return p is SerialPortAddress;
        }

        public bool RecognizeDevice(DeviceAddress p)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new NotSupportedException("Only Windows is supported");

            if (!CheckCriteria(p))
                throw new InvalidException("This recognizer supports only SerialPort Address as input");

            using (ManagementClass i_Entity = new("Win32_PnPEntity"))
            {
                const string CUR_CTRL = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\";

                foreach (ManagementObject i_Inst in i_Entity.GetInstances())
                {
                    var o_Guid = i_Inst.GetPropertyValue("ClassGuid");
                    if (o_Guid == null || o_Guid.ToString()?.ToUpper() != "{4D36E978-E325-11CE-BFC1-08002BE10318}")
                        continue; // Skip all devices except device class "PORTS"

                    var s_DeviceID = i_Inst.GetPropertyValue("PnpDeviceID").ToString();
                    var s_RegEnum = CUR_CTRL + "Enum\\" + s_DeviceID + "\\Device Parameters";
                    var s_PortName = Registry.GetValue(s_RegEnum, "PortName", "")?.ToString();

                    // Check if ID exists
                    if (s_DeviceID == null) continue;

                    // Skip if not a port
                    if (s_PortName == null) continue;

                    // Check if port names are equal
                    if (s_PortName != p.ToString()) continue;

                    // Parse Device ID
                    var parsedId = s_DeviceID.Split("\\", StringSplitOptions.RemoveEmptyEntries);
                    if (parsedId.Length <= 0) continue;

                    // Check if is usb
                    if (parsedId[0] != "USB") continue;

                    // Split VID and PID
                    var vidPid = parsedId[1].Split("&");
                    if (vidPid.Length != 2)
                        throw new InvalidDataException("Cannot parse VID/PID");

                    // Remove trash data
                    var vid = vidPid[0].Replace("VID_", "");
                    var pid = vidPid[1].Replace("PID_", "");

                    // Check if is serial port
                    if (vid == _vid.ToString() && pid == _pid.ToString())
                        return true;

                }
            }

            return false;
        }

        public List<DeviceAddress>? ScanForDevices()
        {
            var addressList = new List<DeviceAddress>();
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new NotSupportedException("Only Windows is supported");

            using (ManagementClass i_Entity = new("Win32_PnPEntity"))
            {
                const string CUR_CTRL = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\";

                foreach (ManagementObject i_Inst in i_Entity.GetInstances())
                {
                    var o_Guid = i_Inst.GetPropertyValue("ClassGuid");
                    if (o_Guid == null || o_Guid.ToString()?.ToUpper() != "{4D36E978-E325-11CE-BFC1-08002BE10318}")
                        continue; // Skip all devices except device class "PORTS"

                    var s_DeviceID = i_Inst.GetPropertyValue("PnpDeviceID").ToString();
                    var s_RegEnum = CUR_CTRL + "Enum\\" + s_DeviceID + "\\Device Parameters";
                    var s_PortName = Registry.GetValue(s_RegEnum, "PortName", "")?.ToString();

                    // Check if ID exists
                    if (s_DeviceID == null) continue;

                    // Skip if not a port
                    if (s_PortName == null) continue;

                    // Parse Device ID
                    var parsedId = s_DeviceID.Split("\\", StringSplitOptions.RemoveEmptyEntries);
                    if (parsedId.Length <= 0) continue;

                    // Check if is usb
                    if (parsedId[0] != "USB") continue;

                    // Split VID and PID
                    var vidPid = parsedId[1].Split("&");
                    if (vidPid.Length != 2)
                        throw new InvalidDataException("Cannot parse VID/PID");

                    // Remove trash data
                    var vid = vidPid[0].Replace("VID_", "");
                    var pid = vidPid[1].Replace("PID_", "");

                    // Check if is serial port
                    if (vid == _vid.ToString() && pid == _pid.ToString())
                        addressList.Add(new SerialPortAddress(s_PortName));

                }

            }

            return addressList;
        }

        public void SetDataExchanger(IDataExchanger dataExchanger)
        {
            // Do nothing (it's not used here)
        }

        public USBSerialPortRecognizer(string vid, string pid)
        {
            _vid = vid;
            _pid = pid;
        }
    }
}

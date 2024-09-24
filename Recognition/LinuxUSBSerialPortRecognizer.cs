using IRIS.Addressing;

namespace IRIS.Recognition
{
    public readonly struct LinuxUSBSerialPortRecognizer(string vid, string pid) : IDeviceRecognizer<SerialPortDeviceAddress>
    {
        // find /sys/bus/usb/devices/usb*/ -name dev
        // split by line
        // (cut /dev from the path)
        // udevadm info -q name -p /sys/bus/usb/devices/usb1/1-1/1-1:1.0/tty/ttyACM0
        // output is device port name
        // Now we've port name ;)
        
        /*
         root@DESKTOP-BNDTFP0:/sys/bus/usb/devices/usb1/1-1/1-1:1.0/tty/ttyACM0/device/tty/ttyACM0/device# find /sys/bus/usb/devices/usb* / -name dev
            /sys/bus/usb/devices/usb1/dev
            /sys/bus/usb/devices/usb1/1-1/dev
            /sys/bus/usb/devices/usb1/1-1/1-1:1.0/tty/ttyACM0/dev
            /sys/bus/usb/devices/usb2/dev
            root@DESKTOP-BNDTFP0:/sys/bus/usb/devices/usb1/1-1/1-1:1.0/tty/ttyACM0/device/tty/ttyACM0/device#
          
         */
        public List<SerialPortDeviceAddress> ScanForDevices()
        {
            throw new NotImplementedException();
        }

        public bool CheckDevice(SerialPortDeviceAddress deviceAddress)
        {
            throw new NotImplementedException();
        }
    }
}
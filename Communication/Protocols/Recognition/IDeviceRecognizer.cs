using IRIS.Communication.Protocols.Addressing;
using IRIS.Devices;

namespace IRIS.Communication.Protocols.Recognition
{
    public interface IDeviceRecognizer
    {
        /// <summary>
        /// Scan for all available devices
        /// </summary>
        List<DeviceAddress>? ScanForDevices();

        /// <summary>
        /// Recognize specified device
        /// </summary>
        bool RecognizeDevice(DeviceAddress p);

        /// <summary>
        /// Check if device address meets recognizer criteria
        /// </summary>
        bool CheckCriteria(DeviceAddress p);

        /// <summary>
        /// Set data exchanger if recognition requires communication
        /// </summary>        
        void SetDataExchanger(IDataExchanger dataExchanger);
    }
}

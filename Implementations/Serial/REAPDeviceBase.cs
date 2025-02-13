using IRIS.Addressing.Ports;
using IRIS.Communication.Serial;
using IRIS.Communication.Serial.Settings;
using IRIS.Devices.Serial;
using IRIS.Protocols.IRIS;

namespace IRIS.Implementations.Serial
{
    public abstract class REAPDeviceBase(SerialPortDeviceAddress deviceAddress,
        SerialInterfaceSettings settings) : SerialDeviceBase(deviceAddress, settings)
    {
        public async Task<uint> SetRegister(uint register, uint value) =>
            await REAP<CachedSerialPortInterface>.SetRegister(HardwareAccess, register, value);
        
        public async Task<uint> GetRegister(uint register) =>
            await REAP<CachedSerialPortInterface>.GetRegister(HardwareAccess, register);
        
    }
}
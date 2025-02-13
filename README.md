# IRIS<sup>2</sup>: Intermediate Resource Integration System V2
IRIS is a framework to implement communication between PC and embedded peripherals.
It's designed to be able to easily implement new devices,
protocols and communication interfaces.

## Requirements
* .NET 8.0+ (Windows 10.0.19041.0+)
* C# 12+

# Generic concepts
## CommunicationInterface
Communication interface represents hardware (or software)
layer used to communicate with device. It can be for 
example a serial port, BLE device but also a custom 
application that acts as proxy between C# and hardware.

Interfaces are used to provide read/write commands (and 
sometimes interface-specific commands) to be used by IRIS 
devices.

## Device (DeviceBase)
Device is a HAL representation of a physical device. 
This is the core of framework, and it's used to implement
specific devices e.g. Heart Rate Band that is BLE Device 
looking for specific GATT services.

Generally you should be most likely interested in this 
subsystem.

## Address
IRIS uses custom address structures to provide 
addressing to communication interfaces. If you're not 
using low-level features you most likely shall use address 
required by device constructor (e.g. 
`SerialPortDeviceAddress`)

## Protocol
Protocols are translation layer - if device requires a 
set of commands to operate e.g. reading confirmation 
after write then protocol is used to provide this 
feature. 

You shall configure protocol with native interface
data and add custom HAL commands to it.

# Usage - devices
## Custom device
To create a custom device you shall implement specific 
DeviceBase (or create your own one) and implement its 
methods.

It is required for **DeviceBase** abstraction to set up 
**HardwareAccess** in constructor.

### Example of simple device:
```csharp
 public sealed class MyExampleBLEDevice() :
  BluetoothLEDevice(GattServiceUuids.HeartRate)
    {
        // Your code
    }
```

### Connection and disconnection
Device requires `Connect` and `Disconnect` methods to be 
present and handle connection and disconnection from the
HardwareAccess. Basic implementation is provided in 
`DeviceBase` class and is to call `HardwareAccess.
Connect` and `HardwareAccess.Disconnect` methods on their
respective calls in Device.

`Connect` method should return true if device is already 
connected and `Disconnect` should return true if device
is already disconnected to prevent confusion.

### Reading and writing to device
You can add custom Read/Write methods to the device that 
access `HardwareAccess` or use protocol as proxy layer to
access hardware.

```cs
// Sending data to device using custom protocol
public async Task SendMessage(string message) =>
    await LINE<CachedSerialPortInterface>.SendMessage
    (HardwareAccess, message);
```

```cs
// Sending data to device using (made-up) hardware access
public async Task WriteString(string message) =>
    await HardwareAccess.WriteAsync(Encoding.ASCII.GetBytes(message));
```

### Device events
If you want to attach device to HardwareAccess events
this should be done in `Connect` and `Disconnect` 
methods or in constructor (depending on event type).

Events related to HardwareAccess and independent of 
device usually should be subscribed
and unsubscribed in constructor/destructor as they are
related to protocol. A good example of such case would be 
BLE device advertisement received event.

Event related to Device should be always subscribed and
unsubscribed in `Connect` and `Disconnect` methods.

Methods can be overriden for that exact purpose, when 
overriding `Connect` and `Disconnect` methods always 
return true if device is already in desired state.

## Already Implemented devices
IRIS comes with several already implemented devices:

### SerialDeviceBase
SerialDevice is used to communicate with serial devices on
COM ports. It uses `CachedSerialPortInterface` to communicate
with hardware - data received from port is automatically
cached to prevent issues with lost data which may occur
when reading from port directly.

It takes two parameters in constructor:
* `SerialPortDeviceAddress` - address of device
* `SerialInterfaceSettings` - settings for serial port
(baud rate, parity, etc.)

Example:
```cs
public sealed class MySerialDevice(
        SerialPortDeviceAddress deviceAddress,
        SerialInterfaceSettings settings) :
        SerialDeviceBase(deviceAddress, settings)
{
    // Code     
}
```

### BluetoothLEDeviceBase
Device used to communicate with BLE devices. Requires 
BLE to be present on the system and uses WinRT API to
perform operations.

It takes one of two parameters in constructor:
* `string` - RegEx pattern to match device name (e.g. 
  using prefix - `DEVICE-.*`)
* `Guid` - GATT service UUID to match device

Note: BLE devices are still W.I.P. and many things may 
change, especially initialization of endpoints 
(characteristics).

Examples:
```cs
public sealed class MyBLEDevice() :
    BluetoothLEDeviceBase(GattServiceUuids.HeartRate)
{
    // Code
}
```

```cs
public sealed class MyBLEDevice() :
    BluetoothLEDeviceBase("DEVICE-.*")
{
    // Code
}
```

## Extending DeviceBase
If you want to extend DeviceBase with custom methods you 
simply need to create a new class that inherits from
`DeviceBase` and implement your methods.

DeviceBase requires two type arguments:
`TCommunicationInterface` - interface used to communicate
with device
`TAddress` - address structure used to address device

Those types can be either defined by you or passed from your
device abstraction.

For reference examples you can check already implemented
devices (mentioned above).

# Usage - interfaces
## Already implemented interfaces
IRIS comes with several already implemented interfaces:
### SerialPortInterface
Simple SerialPort interface that uses `System.IO.Ports.
SerialPort`. Definitely not recommended to be used in
production as it doesn't provide any caching mechanism
and may cause issues with lost data.

### CachedSerialPortInterface
Better version of `SerialPortInterface` that caches data
received from port. Used to communicate with devices on
COM ports.

### BluetoothLEInterface
Interface used to communicate with BLE devices. Uses WinRT
API to perform operations. Handles most communication 
and is not recommended to be used directly.

### VirtualInterface
Fake interface useful for testing purposes. It doesn't
communicate with any hardware and is used to simulate
communication with device.

## Abstract interfaces
### IRawDataCommunicationInterface
Interface marking that specific communication interface
is able to send and receive raw data (byte arrays).

Already implemented in `SerialPortInterface`,
`CachedSerialPortInterface` and`VirtualInterface`.

## Custom interface
To create a custom interface you shall implement 
`ICommunicationInterface` interface and implement its
methods.

Interface requires `Connect` and `Disconnect` methods to
be present and handle connection and disconnection from
the device. If device is already in desired state
methods should return `true`.

For reference examples you can check already implemented
interfaces (mentioned above).

# Usage - protocols
## Already implemented protocols
IRIS comes with several already implemented protocols:
### LINE
Simple protocol used to send and receive strings from 
device. Usually used for getting debug logs.

### REAP - Register Encoding Access Protocol
Also, a simple protocol used to read and write 32-bit 
numbers to and from the device using 32-bit addresses.

Device data is formed as follows:
```cs
// Addresing part
(byte)((registerAddress >> 24) & 0xFF),
(byte)((registerAddress >> 16) & 0xFF),
(byte)((registerAddress >> 8) & 0xFF),
(byte)(registerAddress & 0xFF)

// Writing part
(byte)((registerValue >> 24) & 0xFF),
(byte)((registerValue >> 16) & 0xFF),
(byte)((registerValue >> 8) & 0xFF),
(byte)(registerValue & 0xFF)
```

If device data is not provided (it's only address data) 
then device interprets command as `GET` and returns
value from register.

If device data is provided (it's address and value data)
then device interprets command as `SET` and writes value
to register. It returns value written if device did 
respond or register address if it didn't.

### RUSTIC - Remote Universal Simple Transfer Interface
Protocol used to send and receive data from device. It
uses simple string assignments to send and receive data.

Text name is followed by assignment operator and value 
or question mark. Question mark indicates that device is
expected to return current value of the variable.

Commands are required to end with newline character 
(optionally with carriage return).

Commands look like this:
```
MY_VALUE=?
MY_VALUE=123
```

Example communication:
```
-> MY_VALUE=?
<- MY_VALUE=123
-> MY_VALUE=456
<- MY_VALUE=456 (some devices may use MY_VALUE=OK)
```

## Custom protocol
To create custom protocol you need to create `abstract` 
class that implements `IProtocol` interface and its
methods.

Protocol requires two type parameters
* `TCommunicationInterface` - interface used for
  communication (can be abstraction for wider coverage like 
`IRawDataCommunicationInterface`)
* `TDataType` - data type transmitted to and from communication
  interface (e.g. `byte[]` or `string`), acts as proxy 
  layer between HAL data in custom methods and low-level 
  data in protocol send/receive methods.

Protocol has two base methods - `SendData` and 
`ReceiveData`. Those methods are used as in-class proxy 
layer to send and receive data from communication 
interface.

You can add custom methods to implement protocol-specific
logic like `GetProperty` or `WriteLine`.

For reference examples you can check already implemented
protocols (mentioned above).

# Usage - watchers
Watchers are a quasi-obsolete feature used to find devices
that are connected to the system.

To use watcher you need to create a new instance of
desired watcher and subscribe to its events 
(`OnDeviceAdded`, `OnDeviceRemoved`), then you can `Start` or
`Stop` watcher.

Watcher takes three type arguments:
`TSelf` - type of watcher
`TSoftwareAddress` - address structure used to address 
device in software (e.g. COM port name)
`THardwareAddress` - address structure used to address
device in hardware (e.g. USB VID/PID of your device if 
using USB CDC)

## Already implemented watchers
IRIS comes with several already implemented watchers:

### SerialPortDeviceWatcher
Used to find devices on COM ports.

### WindowsUSBSerialPortDeviceWatcher
Used to find USB CDC devices on Windows and get their
COM port names.

## Appendix
Generally watchers are working, but they are not
recommended to be used as they are not actively maintained
and may be removed in future versions of IRIS for some more
modern solution (e.g. interface that accesses any COM 
device by VID/PID)


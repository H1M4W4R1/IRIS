# IRIS<sup>2</sup>: Intermediate Resource Integration System
IRIS is a framework to implement communication between PC and embedded peripherals.
It's designed to be able to easily implement new devices, protocols and communication interfaces.

## Requirements
* .NET 8.0+ (Windows 10.0.19041.0+)
* C# 12+

## Subpackages
* [IRIS.Serial](https://github.com/H1M4W4R1/IRIS.Serial) - 
Serial communication via COM ports
* [IRIS.Bluetooth.Windows](https://github.com/H1M4W4R1/IRIS.Bluetooth.Windows) - Bluetooth 
communication on Windows (using WinRT API)
* [IRIS.Intiface](https://github.com/H1M4W4R1/IRIS.Intiface) -
Communication with external devices via 
[Intiface Engine](https://github.com/intiface/intiface-engine)

# Generic concepts
## CommunicationInterface
Communication Interfaces are subsystems that are responsible for low-level communication with the device -
it can for example read/write data to OS Serial Port. Role of the Communication Interface is to provide
low-level communication methods that take data and pass it to the device.

Most devices that are already implemented in IRIS have additional abstraction over their communication interface,
and you should not worry about it. If you're implementing custom device you should use one of already implemented
communication interfaces or implement your own.

## Device (DeviceBase)
Device is a physical (or virtual) device that is connected to the computer via USB, Bluetooth or other means.
It is a high-abstraction access point to communicate with the device. Device uses ICommunicationInterface to
process transactions between the device and the computer.

It's core framework to implement custom devices using IRIS framework and considered the highest level of abstraction
in the framework. 

**You should be most likely interested in this subsystem.**

## Address
IRIS uses custom address structures to provide addressing to communication interfaces. If you're not 
using low-level features you most likely shall use address required by device constructor (e.g. `SerialPortDeviceAddress`)

## Protocol
Protocol is a data exchange format that defines the communication rules between the
device and C# API. It is used as low level abstraction layer that is performing
communication with the device.

You shall configure protocol with native interface data and add custom HAL commands to it.

# Usage - devices
## Using already implemented devices
To use already implemented device you need to instantiate it via constructor and connect to it.

```cs
// Create device
BLE_HeartBand heartBand = new();

// Connect to device
if(await heartBand.Connect())
    Console.WriteLine("Connected to device");
else
    Console.WriteLine("Failed to connect to device");

// Check if device is connected
Console.WriteLine("Device is connected: " + heartBand.IsConnected);

// Disconnect from device
if(await heartBand.Disconnect())
    Console.WriteLine("Disconnected from device");
else
    Console.WriteLine("Failed to disconnect from device");
```

Then you can check device implementation for specific methods to read/write data from device.

```cs
// Attach to OnHeartRateReceived event
heartBand.OnHeartRateReceived += (HeartRateReadout data) =>
{
    Console.WriteLine("Heart rate received: " + data.HeartRate);
};
```

Note: most devices will have very specific implementations. It is recommended to check them directly in their
respective source files.

## Custom device - simple way
To create a custom device you shall implement specific 
DeviceBase (or create your own one) and implement its 
methods.

It is required for **DeviceBase** abstraction to set up **HardwareAccess** in constructor, if you're implementing
a base device class it will most likely already set up the hardware access in its constructor.

BLE devices from `IRIS.Bluetooth.Windows` automatically set-up hardware access in their constructors, so you don't 
need to worry about it.
```csharp
 public sealed class MyExampleBLEDevice() :  BluetoothLEDevice(GattServiceUuids.HeartRate)
    {
        // Your code
    }
```

Note: Completely implemented devices should be sealed and not extensible (only way to extend them should be by 
static classes and extension methods).

## Custom device - hard way
If you want to create a custom device you shall implement `DeviceBase` class and implement its methods.

DeviceBase requires two type arguments:
* `TCommunicationInterface` - interface used to communicate
  with device
* `TAddress` - address structure used to address device

Those types can be either defined by you or passed from your device abstraction and custom device example is shown 
below:

```cs
// Custom serial device example
// You can use CachedSerialPortInterface from IRIS.Serial package
public class MyDevice : DeviceBase<CachedSerialPortInterface, SerialPortDeviceAddress>
{
    public MyDevice(CachedSerialPortInterface hardwareAccess, SerialPortDeviceAddress address)
    {
        // You must set-up hardware access in EACH device constructor (it's recomended at the beginning of constructor)
        HardwareAccess = hardwareAccess;
        
        // Your code
    }
}
```

Devices support `HardwareAccess` and `RawHardwareAccess` properties that allow to access the communication interface
and send/receive data from the device. The `HardwareAccess` property is used to send and receive data using direct methods
on specified communication interface (e.g. if used for Bluetooth) and `RawHardwareAccess` is used to send and receive raw
data from the device using `IRawDataCommunicationInterface` interface overloads.

If device does not support RawHardwareAccess property then accessing it will throw <b>NotSupportedException</b>. To override this
behavior you can check if HardwareAccess is not null and implements IRawDataCommunicationInterface using is operator.

For more references you can see already implemented devices and their bases in sub-packages.

### Connection and disconnection
Device requires `Connect` and `Disconnect` methods to be present and handle connection and disconnection from the
HardwareAccess. Basic implementation is provided in `DeviceBase` class and is to call `HardwareAccess.
Connect` and `HardwareAccess.Disconnect` methods on their respective calls in Device.

`Connect` method should return true if device is already connected and `Disconnect` should return true if device
is already disconnected to prevent confusion.

### Reading and writing to device
You can add custom Read/Write methods to the device that access `HardwareAccess` or use protocol as proxy layer to
access hardware.

```cs
// Sending data to device using custom protocol
public async ValueTask SendMessage(string message) =>
    await LINE<CachedSerialPortInterface>.SendMessage(HardwareAccess, message);
```

```cs
// Sending data to device using (made-up) hardware access
public async ValueTask WriteString(string message) =>
    await HardwareAccess.WriteAsync(Encoding.ASCII.GetBytes(message));
```

### Device events
If you want to attach device to HardwareAccess events this should be done in `Connect` and `Disconnect` 
methods or in constructor (depending on event type).

Events related to HardwareAccess and independent of device usually should be subscribed and unsubscribed in 
constructor/destructor as they are related to protocol. A good example of such case would be BLE device advertisement
received event.

Event related to Device should be always subscribed and unsubscribed in `Connect` and `Disconnect` methods. 
Those methods sometimes are sealed thus you should use device-specific methods to handle attaching and
detaching to/from events (e.g. for BluetoothLEDeviceBase you should use endpoints feature).

Methods can be overriden for that exact purpose, when overriding `Connect` and `Disconnect` methods always 
return true if device is already in desired state.


# Usage - interfaces
## VirtualInterface
Fake interface useful for testing purposes. It doesn't communicate with any hardware and is used to simulate
communication with device.

## Abstract interfaces
### IRawDataCommunicationInterface
Interface marking that specific communication interface is able to send and receive raw data (byte arrays).

Already implemented in `SerialPortInterface`, `CachedSerialPortInterface` and`VirtualInterface` from
`IRIS.Serial` package.

## Custom interface
To create a custom interface you shall implement `ICommunicationInterface` interface and implement its methods.
Creating an interface requires low-level knowledge of device protocol used and is not recommended for beginners.

Interface requires `Connect` and `Disconnect` methods to be present and handle connection and disconnection from
the device. If device is already in desired state methods should return `true`.

For reference examples you can check already implemented interfaces from sub-packages.

# Usage - protocols
## Already implemented protocols
IRIS comes with several already implemented protocols:
### LINE
LINE protocol is a simple ASCII based protocol that is commonly used for debugging
connections as it only reads and writes lines of text from/to the device. Each line
is separated by new line with optional carriage return.

This protocol is good to be used with ESP32 UART logs.

Example commands can be seen below:
* `Hello, World!` - sends `Hello, World!` to the device

### REAP - Register Encoding Access Protocol
Also, a simple protocol used to read and write 32-bit numbers to and from the device using 32-bit addresses.

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

If device data is not provided (it's only address data) then device interprets command as `GET` and returns
value from register.

If device data is provided (it's address and value data) then device interprets command as `SET` and writes value
to register. It returns value written if device did respond or register address if it didn't.

### RUSTIC
RUSTIC protocol is a simple ASCII based protocol that is used to communicate with
usage of commands. It is used to send and receive data from the device. Each command
consists of property name, equals sign and value. Commands are separated by new line with
optional carriage return. Question mark value is used to get property value.

Example commands can be seen below:
* `PROPERTY=VALUE` - sets property to desired value
  * E.g. `LED=1`
  * E.g. `LED=ON`
  * E.g. `LED=BLINK`
  * Good Response: `LED=OK`
  * Good Response: `LED=1`
  * Error Response: `ERR=UNKNOWN_COMMAND`
  * Error Response: `ERR=INVALID_VALUE`
* `PROPERTY=?` - gets property value
  * E.g. `LED=?`
  * Good Response: `LED=1`
  * Good Response: `LED=ON`
  * Good Response: `LED=BLINK`
  * Error Response: `ERR=UNKNOWN_COMMAND`
  * Error Response: `ERR=INVALID_VALUE`

## Custom protocol
To create custom protocol you need to create `abstract` class that implements `IProtocol` interface and its
methods.

Protocol requires two type parameters
* `TCommunicationInterface` - interface used for
  communication (can be abstraction for wider coverage like 
`IRawDataCommunicationInterface`)
* `TDataType` - data type transmitted to and from communication
  interface (e.g. `byte[]` or `string`), acts as proxy 
  layer between HAL data in custom methods and low-level 
  data in protocol send/receive methods.

Protocol has two base methods - `SendData` and `ReceiveData`. Those methods are used as in-class proxy 
layer to send and receive data from communication interface.

You can add custom methods to implement protocol-specific logic like `GetProperty` or `WriteLine`.

For reference examples you can check already implemented protocols (mentioned above).

## Using Watchers
To use Watchers, you need to subscribe to `OnDeviceAdded` and `OnDeviceRemoved` events.

```C#
// Create new SerialPortDeviceWatcher
SerialPortDeviceWatcher watcher = new SerialPortDeviceWatcher();
watcher.OnDeviceAdded += OnDeviceAddedCallback;
watcher.OnDeviceRemoved += OnDeviceRemovedCallback;
```

Then you need to start the watcher to begin scanning for devices. Watchers create own task for scanning devices
and work in the background.

```C#
// Start the watcher
watcher.Start();

// Code goes here

// Stop the watcher
watcher.Stop();
```

To get all devices currently detected by Watcher you can use `AllSoftwareDevices` or `AllHardwareDevices` properties.
Software devices are software identifiers of the device (e.g. COM port name), hardware devices are hardware identifiers
of the device (e.g. VID and PID). This allows to have two different types of identifiers for the same device to handle
those rare use cases when we want to scan for all devices and know their hardware identifiers.

## Adding custom Watchers
To add custom watcher you need to extend `DeviceWatcherBase<TSelf, TSoftwareAddress, THardwareAddress>` or
`DeviceWatcherBase<TSelf, TDeviceAddress> class and implement `ScanForDevicesAsync` method.

```C#
public class CustomDeviceWatcher : DeviceWatcherBase<CustomDeviceWatcher, string, string>
{
    protected override ValueTask<(List<string>, List<string>)> ScanForDevicesAsync(
            CancellationToken cancellationToken)
    {
        // Mock-up result
        List<string> softwareDevices = new List<string> { "COM1", "COM2" };
        List<string> hardwareDevices = new List<string> { "1234:5678", "8765:4321" };
        
        return ValueTask.FromResult((softwareDevices, hardwareDevices));
    }
}
```

Watchers are costly feature as they constantly run scans.
To reduce that price you can increase scan interval via ScanInterval property

## Appendix
Generally watchers are working, but they are not
recommended to be used as they are not actively maintained
and may be removed in future versions of IRIS for some more
modern solution (e.g. interface that accesses any COM 
device by VID/PID)


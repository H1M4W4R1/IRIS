# IRIS: Intermediate Resource Integration System
IRIS is a framework to implement communication between PC and embedded peripherals.
It's designed to support multiple interfaces such as
* USB Serial Port
* REST API over HTTP[S] (nyi)

**Warning: Framework is still WIP (even tho it should work fine)**

# Concepts
API consists of several abstraction levels
## IDevice
`IDevice` is an abstraction representation of physical device and it's functions. 

It's used to send commands and define supported protocols or detect / connect to the device.

## IProtocol
Represents communication protocol parameters for specific protocol. Two example protocols have been implemented.

* RUSTIC - simple command communication protocol like `PARAMETER=VALUE` or `PARAMETER=?`, useful for implementation of simple devices / toys
* FOCUS - more complicated constant response-sized (by default) binary data protocol useful for simple diagnostic devices or more advanced toys

IProtocol should be used to abstract interface-based parameters eg. if interface allows HTTP access or it requires to use secure HTTPS connection.

## IDataExchanger
Represents communication method between device and PC. Can be for example `ReliableSerialPort` or HTTP Client. Is different than protocol, as protocol can support multiple DataExchangers - eg. device can use same protocol over TCP/IP or over USB CDC.

## IDeviceRecognizer
Device recognition object - used to detect all available devices on current machine (eg. scans all USB connections and detects COM Ports on Windows (see `USBSerialPortRecognizer`).

Also can be used to check if specific `DeviceAddress` is a proper address of device.

## DeviceAddress
`DeviceAddress` is an abstraction of device address point eg. COM Port name or HTTP endpoint (or IP address)

Is used to determine all available devices and connect to them using `IDeviceRecognizer` implementation.

## InvokableCommand
Command that can be executed on device. Simple as that.

## IReceivedDataObject
Represents data received from command and is used to decode data into C# object, as command can return data in multiple formats - string, byte array, JSON etc.

# Exceptions
Some exceptions may appear if you implement this API.

## CommunicationException
Thrown when device communication failed (eg. cannot open Serial Port)

## ExecutionException
Thrown when you try to execute command when device is not configured properly.

## HardwareException
Thrown when hardware returns error (eg. non-existing command), to be implemented on command level.

## InvalidException
Thrown when device configuration is not valid (eg. using HTTP Address on SerialPort device)
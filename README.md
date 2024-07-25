# IRIS<sup>2</sup>: Intermediate Resource Integration System V2
IRIS is a framework to implement communication between PC and embedded peripherals.
It's designed to be able to easily implement new devices,
protocols and communication interfaces.

## Important
* Warning: Framework is still WIP (even tho it should 
work fine)
* Version 2 is a complete rewrite of the original IRIS 
framework and is not compatible with it.
* Version 2 was not yet tested in production environment, 


At this moment only interfaces mentioned below are 
implemented, but more are planned.
* USB Serial Port

## Requirements
* .NET 8.0+
* C# 12+

Why? Because I like new stuff... C# 12 makes it way 
easier to make code more readable and maintainable.

# Concepts
API consists of several abstraction levels.

## Device (`DeviceBase`)
Device is a representation of a physical device. 
It consists of protocol and interface used to communicate with it.

## Protocol (`IProtocol`)
Protocol is a representation of a communication protocol used to communicate with device.
It is used to convert data from objects to bytes and vice versa.

## Interface (`ICommunicationInterface`)
Interface is a representation of a communication 
interface used to communicate with device for example Serial Port, REST API, etc.

This interface is responsible for handling all low-level communication with device
like opening, closing, reading and writing data.

## Address (`IDeviceAddress`)
Address is a representation of a device address - for example COM port, IP address, etc.

This is used to identify device on a specific interface.

# Implemented interfaces
## SerialPortInterface
Serial Port Interface is a communication interface used to communicate with devices over serial port.
It can be any COM port device (either USB or RS232).

This interface uses improved implementation of 
`SerialPort` class from `System.IO.Ports` namespace, 
which reduces chances of deadlocks, missed data and other issues.

For reference see `SerialPortInterface` class.
You can also base your own implementation on it.

## VirtualInterface
Virtual Interface is a communication interface used to 
simulate communication with devices. It's useful for 
testing purposes, emulation of devices, etc.

Most of methods are compatible with `SerialPortInterface`.

For reference see `VirtualInterface` class.

# Implemented protocols
## FOCUS
FOCUS is a protocol used to communicate with devices via 
command-response style communication. All commands are 
arrays of bytes, and all responses are fixed length (per 
command type) arrays of bytes with an exception that 
error status can change the length of response to 4 bytes.

Response length includes first status byte and last end 
of line byte (nobody knows why it's there, but it's kept 
for backward compatibility).

For reference see `FocusProtocol` class.

Example command is:
```
0x02 0x00 0x0D 0x0A
``` 
This example comes from an experimental device, where 
0x02 command reads value from ADC port 0x00.

Which then responds with:
```
0x01 0x32 0x00 0x0A
```
Where 0x01 is status byte (in this case OK), 0x32 0x00 is 
value read from ADC (0x3200 = 12800) and 0x0A is end of line.

## RUSTIC
RUSTIC is a simple protocol used to communicate with 
devices via ASCII commands. All commands are 
assignment-like strings.

For example:
```
ADC0=?
```
can be used to read value from ADC0 port and response 
would be:

```
ADC0=12800
```

Alternatively, you can set value of GPIO0 port with:
```
GPIO0=1
```

And response would be either:
```
GPIO0=1
GPIO0=OK
```
depending on device implementation.

For reference see `RusticProtocol` class.

# Exceptions
Some exceptions may appear if you implement this API.

## CommunicationException
Thrown when device communication failed (eg. cannot open Serial Port)

## ExecutionException
Thrown when you try to execute command when device is not configured properly.

## HardwareException
Thrown when hardware returns error (eg. non-existing command), to be implemented on command level.
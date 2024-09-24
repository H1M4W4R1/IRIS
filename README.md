# IRIS<sup>2</sup>: Intermediate Resource Integration System V2
IRIS is a framework to implement communication between PC and embedded peripherals.
It's designed to be able to easily implement new devices,
protocols and communication interfaces.

## Requirements
* .NET 8.0+
* C# 12+

# Concepts
## Devices
Device is a representation of a physical device. 
It uses a specific communication interface to connect to 
the device. You can consider this class a Facade for 
lower-level communication shenanigans.

Device is a high abstraction level that should be 
implemented and used by application to execute 
transactions - even if it's possible to execute 
transaction on the device from HAL level it is not recommended
as it may lead to unexpected behavior in future releases and
is not supported.

See `Examples` for reference implementation of device 
abstractions that are used to execute transactions between
physical device and application.

## Address
Address is a representation of a device address - for example COM port, IP address, etc.

This is used to identify device on a specific interface.

## Communication Interface
Communication Interface is a low-level abstraction of 
transaction execution. It is used to send and receive data
from the device and should contain binary-level communication
as well as methods to open and close the connection.

Communication interface uses Protocol and Transaction to 
encode and decode data sent to and received from the device
and respective data type related to specific transaction 
(either received data structure or data structure to be 
sent).

`SendDataAsync` and `ReceiveDataAsync` methods are
used to send and receive data from the device and are an 
access point for transactions to execute their commands 
through the interface.

For more information see `ICommunicationInterface` interface.

## Protocol
Protocol is a representation of a communication protocol used to
encode and decode data sent to and received from the device.

This acts as a middleman between C# structures and
device-specific binary data allowing for easy 
implementation of multi-protocol devices or multiple devices
that use the same protocol.

There are several example protocols implemented. For 
reference see `Protocols` directory / namespace.

## Transaction 
Transaction is a representation of a single communication 
transaction with device.

It can be a command sent to device, response from device or
data exchange (request and response). Transactions are 
divided into three types: `Write`, `Read` and `Exchange`.

Each of transaction types has its own implementation 
method that is used to execute the transaction.

Also, transactions can be executed using static override.
In such case default structure values are used to execute 
the transaction implementation.

All transactions are inheriting from 
`ITransactionWithRequest` or `ITransactionWithResponse` interfaces
which are used to determine if transaction has request or
response data. Those interfaces provide access layer to 
encode or decode data from the transaction via provided 
protocol. You can also skip protocol type to make 
protocol-independent transaction encoding/decoding, but 
this should be insanely rare case.

Default implementation of transaction encoding/decoding 
is passed directly to protocol implementation.

# Exceptions
Some custom exceptions may appear if you implement this API.

## CommunicationException
Thrown when device communication failed (e.g. cannot 
open Serial Port)

## ExecutionException
Thrown when you try to execute command when device is not configured properly.

## HardwareException
Thrown when hardware returns error (e.g. non-existing 
command), to be implemented on command level.

# Implementation and Usage
## Creating custom protocol
To create custom protocol you need to implement `IProtocol` interface
on your structure and implement `Encode` and `Decode` 
methods.

A good example of protocol implementation is `LineProtocol`
which converts `LineTransactionData` to binary data and
vice versa (converts string to/from byte array of ASCII
characters).

```csharp
public struct MyCustomProtocol : IProtocol
{
    public static byte[] EncodeData<TData>(TData inputData) where TData : struct
    {
        // Implementation
    }

    public static bool DecodeData<TData>(byte[]? inputData, out TData outputData) where TData : struct
    {
        // Implementation
    }
}

```
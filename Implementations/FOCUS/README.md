# FOCUS: Fast Operational Command Utility System

## About
FOCUS is a command-line protocol used to interact with 
external electronic devices. It is designed to be fast 
and easy to implement.

## Protocol
The protocol is based on a simple command-response where 
user sends an array of bytes to the device and the device 
responds with fixed length response. 

## Correct responses
If the device receives a correct command, it will 
respond with a fixed length response where the first 
byte is status byte, then data bytes and finally an 
end-of-line byte ('\n').

## Error responses
If the device receives an incorrect command or command 
processing results in an error, it will respond with a 
4-byte error message where the first byte is status byte, 
then 2 bytes of error code and finally an end-of-line 
byte ('\n').

## Implementation
You should check IFocusCommand interface.

To implement FOCUS you need to create a command struct 
and possibly a response struct. The command struct should 
implement `IFocusCommand` interface and the response struct
should implement `IUnmanaged<TSelf>` interface.

Example command
```cs
    public readonly struct GetValueCmd(byte wireIndex) : IFocusCommand, IUnmanaged<GetValueCmd>
    {
        public byte[] RequestBytes => [CommandIdentifiers.COMMAND_GET_N, wireIndex, CommandIdentifiers.CR, CommandIdentifiers.LF];
        public int ResponseLength { get; } = 4;
        
        public TData Decode<TData>(byte[] data) where TData : struct
        {
            // Create new TData object
            TData readout = new TData();
            
            // Check if TData is of type WireDiagAnalogReadout
            if(readout is not WireDiagAnalogReadout) 
                throw new ConstraintException("Data is not of type WireDiagAnalogReadout");
            
            // Decode data, cast to TData and return
            // Casting should be safe as we have already checked if TData is of type WireDiagAnalogReadout
            // We use object-cast proxy trick to avoid compiler error
            readout = (TData) (object) new WireDiagAnalogReadout(data[0], (ushort) (data[1] << 8 | data[2]));
            return readout;
        }
    }   
```

Example response struct implemented in command parsing
```cs
    public readonly struct WireDiagAnalogReadout(byte status, ushort value) : IUnmanaged<WireDiagAnalogReadout>
    {
        public readonly byte status = status;
        public readonly ushort value = value;
    }
```



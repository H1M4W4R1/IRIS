# RUSTIC: Remote Unit Simple Transmission for Information Commands

## About
RUSTIC is a simple protocol for sending commands to an 
external electronic device. It is designed to be easily
readable and writable by humans, and to be easily 
implemented on a wide variety of platforms.

## Protocol
The protocol is a simple text-based protocol. Each command
is a single line of text, with the command name followed by
equals and then the command value. 

**All commands MUST be terminated with a newline 
character.**

### Set command
Set commands are used to set the value of a parameter on the
device. The command name is the name of the parameter, and
the command value is the new value for the parameter. For
example, to set the device's temperature to 25 degrees, the
command would be:
```
temperature=25
```

The device should respond with a single line of text,
indicating the result of the command. For example, if the
device successfully set the temperature, it would respond
with:
```
temperature=OK
```

To query the current temperature, the command would be:
```
temperature=?
```

### Get command
Get commands are used to query the value of a parameter on
the device. The command name is the name of the parameter,
followed by an equals sign and a question mark. For example,
to query the current temperature, the command would be:
```
temperature=?
```

The device should respond with a single line of text,
indicating the current value of the parameter. For example,
if the current temperature is 25 degrees, the device would
respond with:
```
temperature=25
```

If the device does not support the requested parameter, it
should respond with an error message. For example, if the
device does not support the temperature parameter, it would
respond with:
```
temperature=ERR
```

### Error handling
If the device receives a command that it does not understand,
it should respond with an error message. For example, if the
device receives the command:
```
foo=bar
```
it should respond with:
```
foo=ERR
```
however custom error messages can be implemented and 
thus above example can be:
```
foo=ERR_UNKNOWN_COMMAND
```

### Custom errors
Custom error messages can be implemented by the device. For
example, if the device receives a command that is out of
range, it could respond with:
```
temperature=ERR_OUT_OF_RANGE
```

This allows the device to provide more detailed 
information and feedback to the user / client.

<b>All error messages must start with `ERR_` prefix.</b>


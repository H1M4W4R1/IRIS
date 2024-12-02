# Arduino RUSTIC LED Example
## About
This example allows to implement simple command-based 
interface for your device using IRIS embedded RUSTIC 
protocol. Commands are simple equalities that are
sent to the device and the device responds with 
similar message.


## Operational details
This example supports setting the LED on and off and retrieving
the current state of the LED. The commands received by 
device are:
* Set commands
  * `LED=1` - Turns the LED on
  * `LED=0` - Turns the LED off  
* Get command 
  * `LED=?` - Retrieves the current state of the LED 
    (responds with `LED=1` if LED is on and `LED=0` if LED is off)

## Command responses
Set command may respond with `LED=1` or `LED=0` depending on the
value provided in the command. Alternatively it can respond
with `LED=OK` if the command succeeded. `LED` should be 
changed to property name that is being set.

Get command always responds with command name and the value
of the property.

Any RUSTIC command may respond with `ERR=...` if the 
command is not recognized or if the command is not 
supported by the device or some other error occurred. 

In this case error message is provided after `ERR=`.
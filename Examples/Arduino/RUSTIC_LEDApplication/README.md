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
* Set commands (no response)
  * `LED=1` - Turns the LED on
  * `LED=0` - Turns the LED off
* Get command 
  * `LED=?` - Retrieves the current state of the LED 
    (responds with `LED=1` if LED is on and `LED=0` if LED is off)
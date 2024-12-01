# ArduinoEcho
## About
Example echo device that reads from the serial port and writes back to it.

You need to have the Arduino IDE installed and the board
connected to your computer, then you can upload example
sketch onto the board.

To use the example, you need to create device representation
using IRIS. For the reference you can see `ExampleApp`.

## Operational details
The device reads from the serial port and writes back to it
where end of the message is marked by `\n`.
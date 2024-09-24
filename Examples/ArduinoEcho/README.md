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
where end of the message is marked by `\n`. It uses built-in
line-transmission protocol from IRIS and thus message size
is limited to 128 bytes (plus one byte for null-terminator).

For more reference read `README.md` in 
`IRIS/Protocols/LINE` and refer to files in that directory.
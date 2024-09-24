# FOCUS: Fast Operational Command Utility System

## About
FOCUS is a command-line protocol used to interact with 
external electronic devices. It is designed to be fast 
and easy to implement.

## Protocol
The protocol is based on a simple command-response where 
user sends an array of bytes to the device and the device 
responds with "fixed length" response (except error 
messages). 

## Correct responses
If the device receives a correct command, it will 
respond with a fixed length response where the first 
byte is status byte, then all data bytes according to 
length of message response.

## Error responses
If the device receives an incorrect command or command 
processing results in an error, it will respond with a 
4-byte error message where the first byte is status byte, 
then 2 bytes of error code.



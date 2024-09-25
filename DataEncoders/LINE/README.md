# LINE - Line Interface for Native Exchange
LINE is a basic simple protocol based on `\n` newline 
operator.
Each transaction is considered to be a string that ends with a newline operator.

This protocol is used mostly for debugging usage (e.g. 
to use with MCU debugging port).

**Warning**: This protocol supports only text up to 128 
bytes (including `\r\n` characters). String terminator 
is `\0` and is not included in the string length.
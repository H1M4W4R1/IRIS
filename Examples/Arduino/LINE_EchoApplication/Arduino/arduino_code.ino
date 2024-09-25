#define UART_SPEED 115200
#define STRING_LENGTH 128

char serialReadString[STRING_LENGTH + 1];
uint8_t serialReadStringIndex = 0;

void setup() {
    // Initialize serial communication at a baud rate of 9600
    Serial.begin(UART_SPEED);

    // Wait for the serial communication to establish (optional but useful)
    while (!Serial) {
        // Wait until the serial connection is established
    }
}

void loop() {
    // Check if there is any incoming data
    if (Serial.available() > 0) 
    {  
        // Read the incoming data and store it in echo variable
        char incomingByte = Serial.read();
        serialReadString[serialReadStringIndex] = incomingByte;
        serialReadStringIndex++;

        // If end line was received or buffer is about to overflow
        if(incomingByte == '\n' || serialReadStringIndex == STRING_LENGTH)
        {
            // Terminate the string
            serialReadString[serialReadStringIndex] = '\0';

            // Send echo and reset index to start new string
            Serial.print(serialReadString);
            serialReadStringIndex = 0;
        }
    }
}
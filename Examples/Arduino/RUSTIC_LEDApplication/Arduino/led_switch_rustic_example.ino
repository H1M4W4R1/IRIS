#define UART_SPEED 115200
#define DATA_CACHE 128

#define CMD_GET 0
#define CMD_SET 1

#define LED_CMD 				"LED\0"

uint8_t ledValue = LOW;

uint8_t serialDataBuffer[DATA_CACHE];
uint8_t serialDataBufferIndex = 0;

// Command structure
typedef struct {
	uint8_t *name;
  uint32_t cmdValue;
  uint8_t type;
} parsed_cmd_t;


parsed_cmd_t cmd_parse(uint8_t *data)
{
  parsed_cmd_t cmd = {
      0
  };
  uint8_t *ptr = data;

  // Skip leading empty characters because ESP32 sucks
  while(*ptr == '\0') { ptr++; }

  cmd.name = ptr; // Save pointer to name start

  // Find name
  while (*ptr != '=' && *ptr != '\0')
      ptr++;

  // Terminate name string
  if (*ptr == '=') { *ptr++ = '\0'; }

  // Check for GET type
  if (*ptr == '?')
  {
      cmd.type = CMD_GET;
      ptr++;
  }
  else
  {
      cmd.type = CMD_SET;

      // Parse CMD text value to uint32_t
      cmd.cmdValue = strtoul((char*) ptr, NULL, 10);
  }

  // Terminate value string (if any) at CR LF
  while (*ptr != '\r' && *ptr != '\n' && *ptr != '\0') { ptr++; }
  *ptr = '\0';

  return cmd;
}

void cmd_process(const parsed_cmd_t *cmd)
{
  // Check if command requested to change LED value
  if (strcmp((char *) cmd->name, LED_CMD) == 0)
  {   
    if(cmd->type == CMD_SET)
    {
      // Invert LED state
      ledValue = cmd->cmdValue ? HIGH : LOW;

      // Update LED display
      digitalWrite(LED_BUILTIN, ledValue);

      // Respond as success
      Serial.print("LED=OK\r\n");
    }
    else if(cmd->type == CMD_GET)
    {
      Serial.print("LED=");
      Serial.print(ledValue);
      Serial.print("\r\n");
    }
  }
  else
  {
    Serial.print("CMD=ERR_UNKNOWN_COMMAND\r\n");
  }
}

void setup() {
  // Initialize serial communication at a baud rate of 9600
  Serial.begin(UART_SPEED);

  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN, LOW);

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
    uint8_t incomingByte = Serial.read();

    // Skip non-ASCII bytes
    if(incomingByte >= 128) return;

    serialDataBuffer[serialDataBufferIndex] = incomingByte;
    serialDataBufferIndex++;

    if(incomingByte == '\n')
    {
      // Write string terminator
      serialDataBuffer[serialDataBufferIndex] = '\0';
      serialDataBufferIndex++;
      parsed_cmd_t cmd = cmd_parse(serialDataBuffer);

      cmd_process(&cmd);
      serialDataBufferIndex = 0;
    }
  }
}
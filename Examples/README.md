# Examples
## Examples structure
Each example is a separate directory that contains the following files:
- `README.md` - description of the example
- `ExampleApp.cs` - example application that demonstrates how to use the library
- Respective device representation e.g. `ExampleArduinoEchoDevice.cs`

## Running examples the fast way
Just create a simple console app and run the example application in the `Main` method.

It is strongly recommended to wait for the user to press 
a key before the application exits as the library uses asynchronous operations.
```csharp
/// <summary>
/// Entry point of the application
/// </summary>
 private static void Main(string[] args)
{
    // Ask user to enter COM port
    retry_com_port:
    Console.WriteLine("Please enter a COM port: ");
    Console.Write("> ");
    string? comPort = Console.ReadLine();
    if (comPort == null) goto retry_com_port;
    
    // Run example application
    ExampleApp.RunApp(comPort);
    
    // Wait for user to press 'C' key
    while (Console.ReadKey().Key != ConsoleKey.C)
    {
        // Do nothing
    }
}
```


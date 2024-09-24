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
    // Run example application
    ExampleApp.RunApp();
    
    // Wait for user to press 'C' key
    while (Console.ReadKey().Key != ConsoleKey.C)
    {
        // Do nothing
    }
}
```


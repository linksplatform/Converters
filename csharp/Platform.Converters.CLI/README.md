# Platform Converters CLI

A command-line tool for file conversion with plugin support, implementing the solution for [issue #67](https://github.com/linksplatform/Converters/issues/67).

## Features

- **Plugin System**: Dynamically load converter DLL plugins
- **Multiple Converters**: Support for various file format conversions
- **CLI Interface**: Easy-to-use command-line interface
- **Extensible**: Add new converters by implementing the `IFileConverter` interface

## Installation

Build the project using .NET 8:

```bash
dotnet build Platform.Converters.CLI.csproj
```

## Usage

### Basic Commands

```bash
# Show help
dotnet Platform.Converters.CLI.dll help

# List available converters
dotnet Platform.Converters.CLI.dll list

# Convert a file
dotnet Platform.Converters.CLI.dll convert input.json output.xml

# Convert with specific converter
dotnet Platform.Converters.CLI.dll convert input.json output.xml --converter "JSON to XML Converter"

# Load a plugin
dotnet Platform.Converters.CLI.dll load MyPlugin.dll
```

### Environment Variables

- `CONVERTERS_PLUGIN_DIR`: Directory to load plugins from (default: `./plugins`)

## Plugin Development

### Creating a Converter Plugin

1. Create a new .NET library project
2. Reference the `Platform.Converters` project
3. Implement the `IFileConverter` interface:

```csharp
using Platform.Converters;
using System.IO;
using System.Threading.Tasks;

public class MyConverter : IFileConverter
{
    public string Name => "My Custom Converter";
    public string Description => "Converts format A to format B";
    public string[] SupportedSourceExtensions => new[] { ".a" };
    public string[] SupportedTargetExtensions => new[] { ".b" };

    public bool CanConvert(string sourceExtension, string targetExtension)
    {
        return sourceExtension == ".a" && targetExtension == ".b";
    }

    public async Task ConvertAsync(string sourcePath, string targetPath)
    {
        // File-based conversion implementation
    }

    public async Task ConvertAsync(Stream sourceStream, Stream targetStream, 
        string sourceExtension, string targetExtension)
    {
        // Stream-based conversion implementation
    }
}
```

4. Build the plugin DLL
5. Copy to the plugins directory or load using the `load` command

## Built-in Converters

The example plugins project includes:

- **JSON to XML Converter**: Converts JSON files to XML format
- **XML to JSON Converter**: Converts XML files to JSON format
- **Text to Uppercase Converter**: Converts text files to uppercase
- **Text to Lowercase Converter**: Converts text files to lowercase

## Architecture

```
Platform.Converters.CLI/
├── Program.cs              # Main CLI entry point
├── PluginLoader.cs         # Plugin loading system
└── IFileConverter.cs       # Converter interface (in Platform.Converters)

Platform.Converters.Plugins.Examples/
├── JsonToXmlConverter.cs   # JSON ↔ XML conversion
├── XmlToJsonConverter.cs   
├── TextCaseConverter.cs    # Text case conversion
└── ...

Platform.Converters.CLI.Tests/
├── PluginLoaderTests.cs    # Unit tests for plugin loader
├── ConverterTests.cs       # Unit tests for converters
└── ...
```

## Examples

See the `examples/` directory for sample files and usage scenarios.

## Testing

Run the test suite:

```bash
dotnet test Platform.Converters.CLI.Tests/Platform.Converters.CLI.Tests.csproj
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Add your converter implementation
4. Add tests for your converter
5. Submit a pull request

## License

This project follows the same license as the Platform.Converters project.
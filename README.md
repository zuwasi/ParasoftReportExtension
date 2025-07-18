# Parasoft C++TEST Report Extension for Visual Studio 2022

This Visual Studio extension allows you to load Parasoft C++TEST report files (SARIF or HTML) directly into the Visual Studio Error List pane.

## Features

- **SARIF Support**: Load SARIF format reports from Parasoft C++TEST
- **HTML Support**: Load HTML format reports from Parasoft C++TEST
- **Error List Integration**: Violations appear in the standard Visual Studio Error List
- **File Navigation**: Double-click on errors to navigate to the source location
- **Multiple Severity Levels**: Support for Error, Warning, and Information levels

## Installation

1. Build the solution in Visual Studio 2022
2. The VSIX file will be generated in the `bin\Release` or `bin\Debug` folder
3. Double-click the VSIX file to install the extension
4. Restart Visual Studio

## Usage

1. Go to **Tools** > **Load Parasoft C++TEST Report**
2. Select your SARIF or HTML report file
3. The violations will be loaded into the Error List pane
4. Double-click on any error to navigate to the source location

## Supported Report Formats

### SARIF (Static Analysis Results Interchange Format)
- Full support for SARIF 2.1.0 format
- Extracts rule information, severity levels, and location data
- Preserves rule descriptions and help information

### HTML Reports
- Supports table-based HTML reports
- Supports list-based HTML reports
- Supports div-based HTML reports
- Flexible parsing to handle various HTML report structures

## File Structure

```
ParasoftReportExtension/
├── Commands/
│   └── LoadReportCommand.cs      # Menu command implementation
├── Models/
│   └── ErrorInfo.cs              # Error data model
├── Parsers/
│   ├── SarifParser.cs            # SARIF file parser
│   └── HtmlParser.cs             # HTML file parser
├── Services/
│   └── ErrorListService.cs      # Error List integration
├── Properties/
│   └── AssemblyInfo.cs
├── ParasoftReportExtensionPackage.cs
├── ParasoftReportExtensionPackage.vsct
├── source.extension.vsixmanifest
└── ParasoftReportExtension.csproj
```

## Development

### Prerequisites
- Visual Studio 2022 with Visual Studio SDK
- .NET Framework 4.8

### Building
1. Open the solution in Visual Studio 2022
2. Build the solution (Ctrl+Shift+B)
3. The VSIX package will be generated in the output folder

### Testing
1. Press F5 to launch the experimental instance of Visual Studio
2. Open a C++ project
3. Use the Tools menu to load a Parasoft report
4. Verify that errors appear in the Error List

## Dependencies

- Microsoft.VisualStudio.SDK (17.0.32112.339)
- Microsoft.VSSDK.BuildTools (17.0.5232)
- HtmlAgilityPack (1.11.54)
- Newtonsoft.Json (13.0.3)

## Contributing

Feel free to submit issues and pull requests to improve the extension.

## License

This project is licensed under the MIT License.

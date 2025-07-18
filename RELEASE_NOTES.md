# Parasoft C++TEST Report Extension v1.0.0

## 🎉 Initial Release

The Parasoft C++TEST Report Extension allows you to load static analysis reports from Parasoft C++TEST directly into Visual Studio 2022's Error List pane.

## ✨ Features

- **📊 SARIF Support**: Load SARIF 2.1.0 format reports with full metadata
- **🌐 HTML Support**: Parse HTML reports with intelligent table, list, and div-based extraction
- **📝 Error List Integration**: Violations appear in the standard Visual Studio Error List
- **🎯 File Navigation**: Double-click errors to jump directly to source code locations
- **⚡ Multiple Severity Levels**: Support for Error, Warning, and Information levels
- **🔍 Rule Information**: Displays rule IDs, descriptions, and categories

## 🛠️ Supported Report Formats

### SARIF (Static Analysis Results Interchange Format)
- ✅ SARIF 2.1.0 specification compliant
- ✅ Extracts rule metadata and severity levels
- ✅ Preserves location information (file, line, column)
- ✅ Supports multiple runs and tools in a single file

### HTML Reports
- ✅ Table-based HTML reports
- ✅ List-based HTML reports  
- ✅ Div-based HTML reports
- ✅ Flexible parsing for various HTML structures
- ✅ Regex-based extraction for unstructured content

## 📋 System Requirements

- **Visual Studio**: 2022 (Community, Professional, or Enterprise)
- **OS**: Windows 10/11 (64-bit)
- **.NET Framework**: 4.8 or higher
- **Architecture**: x64 (AMD64)

## 🚀 What's New in v1.0.0

- Initial release with full SARIF and HTML parsing support
- Integrated Error List provider with file navigation
- Robust error handling and user feedback
- Comprehensive documentation and examples

## 🐛 Bug Fixes

- N/A (Initial release)

## 📚 Documentation

- [Installation Guide](INSTALLATION.md)
- [User Guide](USER_GUIDE.md)
- [README](README.md)

## 🤝 Contributing

Found a bug or have a feature request? Please open an issue on our [GitHub repository](https://github.com/zuwasi/ParasoftReportExtension).

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

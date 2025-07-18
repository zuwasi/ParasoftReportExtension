# Installation Guide

## 📦 Download and Install

### Method 1: Install from VSIX File (Recommended)

1. **Download the VSIX file**
   - Download `ParasoftReportExtension.vsix` from the [latest release](https://github.com/zuwasi/ParasoftReportExtension/releases/latest)

2. **Install the extension**
   - **Option A**: Double-click the `.vsix` file
   - **Option B**: In Visual Studio, go to **Extensions** → **Manage Extensions** → **Install from VSIX**

3. **Restart Visual Studio**
   - Close and reopen Visual Studio 2022 to activate the extension

### Method 2: Install from Visual Studio Marketplace

*Coming soon - extension will be published to the Visual Studio Marketplace*

## ✅ Verify Installation

1. Open Visual Studio 2022
2. Go to **Tools** menu
3. You should see **"Load Parasoft C++TEST Report"** option

## 🔧 System Requirements

Before installing, ensure your system meets these requirements:

### Required
- **Visual Studio 2022** (any edition: Community, Professional, Enterprise)
- **Windows 10/11** (64-bit)
- **.NET Framework 4.8** or higher

### Recommended
- **Visual Studio 2022 Version 17.0** or later
- **Administrative privileges** (for installation)

## 🚨 Troubleshooting

### Extension Not Appearing in Tools Menu

1. **Check Extension Status**
   - Go to **Extensions** → **Manage Extensions**
   - Find "Parasoft C++TEST Report Loader" in the **Installed** tab
   - Ensure it's **Enabled**

2. **Restart Visual Studio**
   - Close all Visual Studio instances
   - Reopen Visual Studio

3. **Check Visual Studio Version**
   - This extension requires Visual Studio 2022 (version 17.0 or later)
   - Earlier versions are not supported

### Installation Fails

1. **Run as Administrator**
   - Right-click Visual Studio and select "Run as administrator"
   - Try installing the VSIX again

2. **Check Dependencies**
   - Ensure .NET Framework 4.8 is installed
   - Update Visual Studio to the latest version

3. **Clear Extension Cache**
   - Close Visual Studio
   - Delete: `%LOCALAPPDATA%\Microsoft\VisualStudio\17.0_[instance]\Extensions`
   - Restart Visual Studio and try again

### Extension Crashes or Errors

1. **Check Activity Log**
   - Open Visual Studio with: `devenv /log`
   - Check the activity log for error details

2. **Reset Extension Settings**
   - Go to **Tools** → **Options** → **Environment** → **Extensions**
   - Reset extension settings

## 🔄 Updating

### Automatic Updates
- If installed from the Visual Studio Marketplace, updates will be automatic

### Manual Updates
1. Download the latest VSIX file from GitHub releases
2. Uninstall the current version:
   - **Extensions** → **Manage Extensions** → Find extension → **Uninstall**
3. Restart Visual Studio
4. Install the new version using the same process

## 🗑️ Uninstalling

1. Open Visual Studio 2022
2. Go to **Extensions** → **Manage Extensions**
3. Find "Parasoft C++TEST Report Loader"
4. Click **Uninstall**
5. Restart Visual Studio to complete removal

## 📞 Support

If you encounter issues:

1. **Check the logs** in Visual Studio Activity Log
2. **Search existing issues** on [GitHub](https://github.com/zuwasi/ParasoftReportExtension/issues)
3. **Create a new issue** with:
   - Visual Studio version
   - Error messages
   - Steps to reproduce

## 🎯 Next Steps

Once installed, see the [User Guide](USER_GUIDE.md) to learn how to use the extension.
